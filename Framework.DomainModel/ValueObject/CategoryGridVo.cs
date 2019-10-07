namespace Framework.DomainModel.ValueObject
{
    public class CategoryGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public string ParentName { get; set; }
        public string Background { get; set; }
    }
}