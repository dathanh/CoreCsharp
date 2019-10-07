using ProjectName.Models.Base;

namespace ProjectName.Models.Config
{
    public class DashboardConfigDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Config>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardConfigShareViewModel>(parameters);
        }
    }
}