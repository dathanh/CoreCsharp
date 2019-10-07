namespace Framework.DomainModel.ValueObject
{
    public class PlayListGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int OrderNumber { get; set; }
    }
}
