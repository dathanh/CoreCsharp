using Framework.DataAnnotations;
using Framework.DomainModel.ValueObject;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel.Entities
{
    /// <summary>
    /// User entity
    /// </summary>
    public class User : Entity
    {

        [LocalizeRequired(FieldName = "User name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public int? UserRoleId { get; set; }

        public bool IsActive { get; set; }

        [LocalizeRequired(FieldName = "Full name")]
        public string FullName { get; set; }

        [LocalizePhone(FieldName = "Phone")]
        public string Phone { get; set; }

        public bool? IsSystemUser { get; set; }

        [LocalizeRequired]
        [LocalizeEmailAddress]
        public string Email { get; set; }
        public byte[] Avatar { get; set; }

        public bool? IsAccountFacebook { get; set; }
        public bool? IsAccountGoogle { get; set; }

        public string Passport { get; set; }
        public string PassportImageId { get; set; }

        public int? StatusId { get; set; }

        [NotMapped]
        public int? OldRoleId { get; set; }
        [NotMapped]
        public AppRole AppRole { get; set; }
        [NotMapped]
        public virtual ICollection<GridConfig> GridConfigs { get; set; } = new List<GridConfig>();
        public virtual UserRole UserRole { get; set; }
        [NotMapped]
        public bool IsWebProjectUser { get; set; }

    }
}
