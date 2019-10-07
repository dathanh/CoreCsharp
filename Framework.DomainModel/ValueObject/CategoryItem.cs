using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class CategoryItem : DtoBase
    {
        public string Background { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsFollow { get; set; }
        public int NumOfFollow { get; set; }
        public decimal ViewNumberValue { get; set; }
        public string ViewNumber => ViewNumberValue.ToString("N0");
        public string NameUrl => Name.GetUrlViaName();
    }
}
