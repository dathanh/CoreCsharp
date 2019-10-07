using ProjectName.Models.Base;

namespace ProjectName.Models.MyProfile
{
    public class DashboardMyProfileDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.User>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardMyProfileShareViewModel>(parameters);
        }
    }
}