using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository : IEntityFrameworkRepository<Category>, IQueryableRepository<Category>
    {

        List<CategoryItem> GetListCategoryForBrowsePage(int page, int languageId);
        int GetTotalCategoryForBrowsePage();
        List<CategoryItem> GetListSubCategoryForBrowsePage(int page, int id, int languageId, int customerId);
        int GetTotalSubCategoryForBrowsePage(int id);
        List<Category> ListCategoryParent(int languageId);
        List<Category> GetSubCategoryFromListParentId(int languageId, List<int> parentIds);
        bool CheckNameLanguageIsExists(int catLanguageId, string name, int languageId);
        string GetNameParentById(int id, int languageId);
        List<CategoryAndChild> GetAllCategory(int languageId);
        CategoryItem GetCategoryById(int categoryId, int languageId);
    }
}