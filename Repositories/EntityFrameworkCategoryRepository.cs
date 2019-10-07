using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Newtonsoft.Json;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace Repositories
{
    public class EntityFrameworkCategoryRepository : EntityFrameworkTenantRepositoryBase<Category>, ICategoryRepository
    {
        public EntityFrameworkCategoryRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("Description");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            return (from s in GetAll()
                    select new CategoryGridVo
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive,
                        ParentName = s.Parent != null ? s.Parent.Name : "",
                        Description = s.Description,
                        Background = s.Background,
                    }).OrderBy(queryInfo.SortString);
        }

        public bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId)
        {
            return (from o in DataContext.CategoryLanguages
                    where o.LanguageId == languageId && o.Id != catLanguageId && o.Name != null &&
                          o.Name.ToLower() == name.ToLower()
                    select o).Any();
        }

        public List<CategoryItem> GetListCategoryForBrowsePage(int page, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var dataWithCondition = DataContext.Categories.Where(o => o.IsActive == true && o.ParentId == null);
            IQueryable<CategoryItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = dataWithCondition.OrderBy(o => o.Id)
                        .Select(o => new CategoryItem
                        {
                            Id = o.Id,
                            Background = urlCdn + o.Background,
                            Name = o.Name,
                            ViewNumberValue = o.Children.Sum(ch => ch.Videos.Sum(v => v.ViewNumber))
                        });
            }
            else
            {
                query = (from c in dataWithCondition
                         join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on c.Id equals cl.ParentId
                         select new CategoryItem
                         {
                             Id = c.Id,
                             Background = string.IsNullOrWhiteSpace(cl.Background) ? urlCdn + c.Background : urlCdn + cl.Background,
                             Name = cl.Name,
                             ViewNumberValue = c.Children.Sum(ch => ch.Videos.Sum(v => v.ViewNumber))
                         });
            }

            return query.OrderBy(o => o.Id).Skip((page - 1) * ConstantValue.LimitCategories).Take(ConstantValue.LimitCategories).AsNoTracking().ToList();
        }

        public int GetTotalCategoryForBrowsePage()
        {
            return DataContext.Categories.Count(o => o.IsActive == true && o.ParentId == null);
        }

        public List<CategoryItem> GetListSubCategoryForBrowsePage(int page, int id, int languageId, int customerId)
        {
            var userData = DataContext.Customers.FirstOrDefault(o => o.Id == customerId);
            if (userData == null)
            {
                return new List<CategoryItem>();
            }

            var configStr = userData.CategoryConfig;
            var customerConfigData = new List<CustomerCategoryConfigDto>();
            if (!string.IsNullOrWhiteSpace(configStr))
            {
                customerConfigData = JsonConvert.DeserializeObject<List<CustomerCategoryConfigDto>>(configStr) ??
                                     new List<CustomerCategoryConfigDto>();
            }

            var listSubCategoryId = new List<int?>();
            foreach (var item in customerConfigData)
            {
                listSubCategoryId.AddRange(item.SubCategoryIds);
            }

            var listConfigForAllCus = DataContext.Customers.Where(o => o.CategoryConfig != null && o.CategoryConfig != "").Select(o => o.CategoryConfig);
            var listCustomerDataConfigForFollow = listConfigForAllCus.Select(o => JsonConvert.DeserializeObject<List<CustomerCategoryConfigDto>>(o) ??
                                     new List<CustomerCategoryConfigDto>()).ToList();
            var listSubFollow = new List<int?>();
            foreach (var item in listCustomerDataConfigForFollow)
            {
                foreach (var subItem in item)
                {
                    listSubFollow.AddRange(subItem.SubCategoryIds);
                }
            }

            var urlCdn = ImageHelper.GetImageUrlCdn();
            var categoryCondition = DataContext.Categories.Where(o => o.IsActive == true && o.ParentId == id);
            IQueryable<CategoryItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = categoryCondition
                        .Select(o => new CategoryItem
                        {
                            Id = o.Id,
                            Background = urlCdn + o.Background,
                            Name = o.Name,
                            IsFollow = listSubCategoryId.Contains(o.Id),
                            NumOfFollow = listSubFollow.Count(i => i == o.Id)
                        });
            }
            else
            {
                query = (from cs in categoryCondition
                         join csl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on cs.Id equals csl.ParentId
                         select new CategoryItem
                         {
                             Id = cs.Id,
                             Background = string.IsNullOrWhiteSpace(csl.Background) ? urlCdn + cs.Background : urlCdn + csl.Background,
                             Name = csl.Name,
                             IsFollow = listSubCategoryId.Contains(csl.Id),
                             NumOfFollow = listSubFollow.Count(i => i == cs.Id)
                         });
            }

            var skip = (page - 1) * ConstantValue.LimitCategories;
            var take = ConstantValue.LimitCategories;

            return query.OrderBy(o => o.Id).Skip(skip).Take(take).AsNoTracking().ToList();
        }

        public string GetNameParentById(int id, int languageId)
        {
            if (languageId == (int)LanguageKey.English)
            {
                return DataContext.Categories.FirstOrDefault(o => o.Id == id)?.Name;
            }

            return DataContext.CategoryLanguages.FirstOrDefault(cl => cl.LanguageId == languageId && cl.ParentId == id)?.Name;
        }

        public int GetTotalSubCategoryForBrowsePage(int id)
        {
            return DataContext.Categories.Count(o => o.IsActive == true && o.ParentId == id);
        }

        public List<Category> ListCategoryParent(int languageId)
        {
            var condition = DataContext.Categories.Where(o => o.ParentId == null && o.IsActive == true);
            IQueryable<Category> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = condition;
            }
            else
            {
                query = (from o in condition
                         join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on o.Id equals cl.ParentId
                         select new Category
                         {
                             Id = o.Id,
                             Name = cl.Name
                         });
            }

            return query.OrderBy(o => o.Name).AsNoTracking().ToList();
        }

        public List<Category> GetSubCategoryFromListParentId(int languageId, List<int> parentIds)
        {
            var condition = DataContext.Categories.Where(o => o.IsActive == true && o.ParentId != null && parentIds.Contains(o.ParentId.Value));
            IQueryable<Category> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = condition;
            }
            else
            {
                query = (from o in condition
                         join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on o.Id equals cl.ParentId
                         select new Category
                         {
                             Id = o.Id,
                             Name = cl.Name
                         });
            }

            return query.OrderBy(o => o.Name).AsNoTracking().ToList();
        }
        public List<CategoryAndChild> GetAllCategory(int languageId)
        {
            var condition = from c in DataContext.Categories.Where(c => c.IsActive == true && c.ParentId == null) select c;
            var urlCdn = ImageHelper.GetImageUrlCdn();
            IQueryable<CategoryAndChild> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = (from c in condition
                         select new CategoryAndChild
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Childrent = (from sc in DataContext.Categories.Where(sc => sc.IsActive == true && sc.ParentId == c.Id)
                                          select new CategoryAndChild
                                          {
                                              Name = sc.Name,
                                              Id = sc.Id,
                                              Background = urlCdn + sc.Background
                                          }).OrderBy(o => o.Name).AsNoTracking().ToList(),

                         });
            }
            else
            {
                query = (from c in condition
                         join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on c.Id equals cl.ParentId
                         select new CategoryAndChild
                         {
                             Id = c.Id,
                             Name = cl.Name,
                             Childrent = (from sc in DataContext.Categories.Where(sc => sc.IsActive == true && sc.ParentId == c.Id)
                                          join scl in DataContext.CategoryLanguages.Where(scl => scl.LanguageId == languageId) on sc.Id equals scl.ParentId
                                          select new CategoryAndChild
                                          {
                                              Name = scl.Name,
                                              Id = sc.Id,
                                              Background = string.IsNullOrWhiteSpace(scl.Background) ? urlCdn + sc.Background : urlCdn + scl.Background,
                                          }).OrderBy(o => o.Name).AsNoTracking().ToList(),
                         });
            }

            return query.OrderBy(o => o.Name).AsNoTracking().ToList();
        }

        public CategoryItem GetCategoryById(int categoryId, int languageId)
        {
            var urlCdn = ImageHelper.GetImageUrlCdn();
            var condition = DataContext.Categories.Where(c => c.Id == categoryId && c.IsActive == true);

            IQueryable<CategoryItem> query;
            if (languageId == (int)LanguageKey.English)
            {
                query = (from c in condition
                         select new CategoryItem
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Background = urlCdn + c.Background,
                             Description = c.Description
                         });
            }
            else
            {
                query = (from c in condition
                         join cl in DataContext.CategoryLanguages.Where(cl => cl.LanguageId == languageId) on c.Id equals cl.ParentId
                         select new CategoryItem
                         {
                             Id = c.Id,
                             Name = cl.Name,
                             Description = cl.Description,
                             Background = string.IsNullOrWhiteSpace(cl.Background) ? urlCdn + c.Background : urlCdn + cl.Background,
                         });
            }

            return query.AsNoTracking().FirstOrDefault();

        }
    }
}