using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface ICategoryService : IMasterFileService<Category>
    {
        List<CategoryItem> GetListCategoryForBrowsePage(int page, int languageId);

        int GetTotalCategoryForBrowsePage();

        List<CategoryItem> GetListSubCategoryForBrowsePage(int page, int id, int languageId, int customerId);
        int GetTotalSubCategoryForBrowsePage(int id);
        List<Category> ListCategoryParent(int languageId);
        List<Category> GetSubCategoryFromListParentId(int languageId, List<int> parentIds);
        string GetNameParentById(int id, int languageId);
        bool UpdateFollowSubCategory(int subCategoryId, int customerId);
        List<CategoryAndChild> GetAllCategory(int languageId);
        MetaResponse GetSeoInfo(int categoryId, int languageId);
    }
}