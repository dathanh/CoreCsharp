namespace Framework.DomainModel.Entities
{
    public class CustomerVideoWatched : Entity
    {
        public int? VideoId { get; set; }
        public int? CustomerId { get; set; }
        public int? Status { get; set; }
        public int? CurrentDuration { get; set; }

        public virtual Video Video { get; set; }
        public virtual Customer Customer { get; set; }
    }
}