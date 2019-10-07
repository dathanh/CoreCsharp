using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Language : Entity
    {
        public string Name { get; set; }
        public bool? IsDefault { get; set; }

    }
}