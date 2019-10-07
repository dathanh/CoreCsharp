using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{

    public class UserRoleDto : DtoBase
    {
        public string Name { get; set; }
        public List<UserRoleFunctionGridVo> RoleFunctions { get; set; }
    }
}