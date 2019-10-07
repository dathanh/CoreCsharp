using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ILikeVideoService : IMasterFileService<LikeVideo>
    {
        LikeVideoResponse ProcessLikeVideo(LikeVideo likeInfo);
        LikeVideoResponse GetLikeOfVideo(int videoId, int currentCustomerId);
    }
}