using System

namespace Framework.DomainModel.Entities
{
    public class Customer : Entity
    {   
        public string FullName { get; set; }
        public string Phone { get; set; }
        public Datetime DOB { get; set; }
    }
}
