using Framework.DomainModel.Entities;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces.Common
{
    public interface IConfigSystem
    {
        List<Config> ListConfig { get; }
        void RefershListData(bool isRefreshTenant = true);
        string LoginVideo { get; }
        string ChooseCategoryVideo { get; }
        string ChooseSubCategoryVideo { get; }
        string LoginBackground { get; }
        string ChooseCategoryBackground { get; }
        string ChooseSubCategoryBackground { get; }
    }
}
