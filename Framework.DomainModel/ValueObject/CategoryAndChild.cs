using Framework.Utility;
using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class CategoryAndChild : CategoryDto
    {
        public string Background { get; set; }
        public string NameUrl => Name.GetUrlViaName();
        public List<CategoryAndChild> Childrent { get; set; }
    }
}
