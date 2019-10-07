using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerCategoryConfigDto
    {
        public int CategoryId { get; set; }
        public List<int?> SubCategoryIds { get; set; } = new List<int?>();
    }
}
