namespace Framework.DomainModel
{
    public class MongoEntity : Entity
    {
        public object AltId { get; set; }
        public string CreatedByName { get; set; }
        public string LastModifiedByName { get; set; }
    }
}
