using ProjectName.Models.Base;

namespace ProjectName.Models.Series
{
    public class DashboardSeriesDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Series>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardSeriesShareViewModel>(parameters);
        }
    }
}
