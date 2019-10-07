using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces.Common
{
    public interface IMenuExtractData
    {
        MenuViewModel GetMenuViewModel(int idRole);
        void RefreshListData();
        List<UserRoleFunction> LoadUserSecurityRoleFunction(long userRoleId, long documentTypeId);
    }
}
