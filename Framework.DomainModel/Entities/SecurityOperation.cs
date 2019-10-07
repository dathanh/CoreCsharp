using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class SecurityOperation : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; } = new List<UserRoleFunction>();
    }
}
