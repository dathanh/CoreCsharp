
namespace Framework.DomainModel.ValueObject
{
    public class LikeVideoResponse 
    {
        public int CountLike { get; set; }
        public int CountDislike { get; set; }
        public bool HasLike { get; set; }
        public bool HasDislike { get; set; }

    }
}
