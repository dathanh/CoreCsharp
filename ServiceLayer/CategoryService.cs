using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Newtonsoft.Json;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer
{
    public class CategoryService : MasterFileService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICustomerRepository _customerRepository;

        public CategoryService(ITenantPersistenceService tenantPersistenceService, ICategoryRepository categoryRepository,
            ICustomerRepository customerRepository,
            IBusinessRuleSet<Category> businessRuleSet = null)
            : base(categoryRepository, categoryRepository, tenantPersistenceService, businessRuleSet)
        {
            _categoryRepository = categoryRepository;
            _customerRepository = customerRepository;
        }

        public List<CategoryItem> GetListCategoryForBrowsePage(int page, int languageId)
        {
            return _categoryRepository.GetListCategoryForBrowsePage(page, languageId);
        }

        public string GetNameParentById(int id, int languageId)
        {
            return _categoryRepository.GetNameParentById(id, languageId);
        }

        public int GetTotalCategoryForBrowsePage()
        {
            return _categoryRepository.GetTotalCategoryForBrowsePage();
        }

        public List<CategoryItem> GetListSubCategoryForBrowsePage(int page, int id, int languageId, int customerId)
        {
            return _categoryRepository.GetListSubCategoryForBrowsePage(page, id, languageId, customerId);
        }

        public int GetTotalSubCategoryForBrowsePage(int id)
        {
            return _categoryRepository.GetTotalSubCategoryForBrowsePage(id);
        }

        public List<Category> GetSubCategoryFromListParentId(int languageId, List<int> parentIds)
        {
            return _categoryRepository.GetSubCategoryFromListParentId(languageId, parentIds);
        }

        public List<Category> ListCategoryParent(int languageId)
        {
            return _categoryRepository.ListCategoryParent(languageId);
        }

        public bool UpdateFollowSubCategory(int subCategoryId, int customerId)
        {
            var customer = _customerRepository.GetById(customerId);

            if (customer == null)
            {
                throw new UserVisibleException("InvalidData");
            }

            var customerCategoryConfig = customer.CategoryConfig;
            var customerConfigData = new List<CustomerCategoryConfigDto>();
            if (!string.IsNullOrWhiteSpace(customerCategoryConfig))
            {
                customerConfigData = JsonConvert.DeserializeObject<List<CustomerCategoryConfigDto>>(customerCategoryConfig) ??
                                     new List<CustomerCategoryConfigDto>();
            }

            // Get parent id
            var subCategory = _categoryRepository.GetById(subCategoryId);
            if (subCategory == null)
            {
                throw new UserVisibleException("InvalidData");
            }

            var parentId = subCategory.ParentId;
            if (parentId.GetValueOrDefault() == 0)
            {
                throw new UserVisibleException("InvalidData");
            }

            bool isFollow = false;
            //Check ParentId Exist CategoryId
            var checkParentIdExistInConfig = customerConfigData.FirstOrDefault(o => o.CategoryId == parentId);
            if (checkParentIdExistInConfig != null)
            {
                var listSubInCategoryConfig = checkParentIdExistInConfig.SubCategoryIds;
                if (listSubInCategoryConfig.Contains(subCategoryId))
                {
                    listSubInCategoryConfig.Remove(subCategoryId);
                }
                else
                {
                    listSubInCategoryConfig.Add(subCategoryId);
                    isFollow = true;
                }
            }
            else
            {
                var newItem = new CustomerCategoryConfigDto
                {
                    CategoryId = parentId.GetValueOrDefault(),
                    SubCategoryIds = new List<int?> { subCategoryId }
                };
                customerConfigData.Add(newItem);
                isFollow = true;
            }
            customerConfigData = customerConfigData.Where(o => o.SubCategoryIds.Any()).ToList();
            customer.CategoryConfig = JsonConvert.SerializeObject(customerConfigData);
            _customerRepository.Commit();
            return isFollow;

        }
        public List<CategoryAndChild> GetAllCategory(int languageId)
        {
            return _categoryRepository.GetAllCategory(languageId);
        }
        public MetaResponse GetSeoInfo(int categoryId, int languageId)
        {
            var categoryInfo = _categoryRepository.GetCategoryById(categoryId, languageId);
            if (categoryInfo != null)
            {
                return new MetaResponse()
                {
                    Title = categoryInfo.Name,
                    Description = categoryInfo.Description,
                    Image = categoryInfo.Background,
                };
            }
            return new MetaResponse();
        }
    }
}