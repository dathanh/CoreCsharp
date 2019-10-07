namespace Framework.DomainModel.ValueObject
{
    public class ConfigGridVo : ReadOnlyGridVo
    {
        //public string Value { get; set; }
        public string Name { get; set; }
        public string VideoName { get; set; }
        public string Description { get; set; }
    }
}