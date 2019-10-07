using ProjectName.Models.Base;

namespace ProjectName.Models.Banner
{
    public class DashboardBannerDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Banner>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardBannerShareViewModel>(parameters);
        }
    }
}