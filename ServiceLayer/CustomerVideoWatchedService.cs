using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class CustomerVideoWatchedService : MasterFileService<CustomerVideoWatched>, ICustomerVideoWatchedService
    {
        private readonly ICustomerVideoWatchedRepository _customerVideoWatchedRepository;

        public CustomerVideoWatchedService(ITenantPersistenceService tenantPersistenceService, ICustomerVideoWatchedRepository customerVideoWatchedRepository,
            IBusinessRuleSet<CustomerVideoWatched> businessRuleSet = null)
            : base(customerVideoWatchedRepository, customerVideoWatchedRepository, tenantPersistenceService, businessRuleSet)
        {
            _customerVideoWatchedRepository = customerVideoWatchedRepository;
        }

        public void SaveVideoContinueWatching(VideoContinueWatching customerVideoWatchedInfo)
        {
            var videoInfoExist = _customerVideoWatchedRepository.FirstOrDefault(o => o.VideoId == customerVideoWatchedInfo.VideoId && o.CustomerId == customerVideoWatchedInfo.CustomerId);
            if (videoInfoExist == null)
            {
                if (customerVideoWatchedInfo.Status != (int)CustomerVideoStatus.Completed)
                {
                    _customerVideoWatchedRepository.Add(new CustomerVideoWatched
                    {
                        VideoId = customerVideoWatchedInfo.VideoId,
                        CustomerId = customerVideoWatchedInfo.CustomerId,
                        CurrentDuration = customerVideoWatchedInfo.CurrentDuration,
                        Status = customerVideoWatchedInfo.Status,
                    });
                }

            }
            else
            {
                if (customerVideoWatchedInfo.Status == (int)CustomerVideoStatus.Completed)
                {
                    _customerVideoWatchedRepository.DeleteById(videoInfoExist.Id);
                }
                else
                {
                    videoInfoExist.Status = customerVideoWatchedInfo.Status;
                    videoInfoExist.CurrentDuration = customerVideoWatchedInfo.CurrentDuration;

                }

            }
            _customerVideoWatchedRepository.Commit();

        }
    }
}