using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ICustomerVideoWatchedService : IMasterFileService<CustomerVideoWatched>
    {

        void SaveVideoContinueWatching(VideoContinueWatching customerVideoWatchedInfo);
    }
}