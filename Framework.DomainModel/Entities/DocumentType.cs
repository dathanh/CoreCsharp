using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class DocumentType : Entity
    {

        public string Name { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public virtual ICollection<GridConfig> GridConfigs { get; set; } = new Collection<GridConfig>();
        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; } = new List<UserRoleFunction>();
    }
}
