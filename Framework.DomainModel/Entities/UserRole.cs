using Framework.DataAnnotations;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class UserRole : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }

        public string AppRoleName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; } = new List<UserRoleFunction>();
    }
}
