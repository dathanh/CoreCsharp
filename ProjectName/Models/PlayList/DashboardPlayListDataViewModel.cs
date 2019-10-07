using ProjectName.Models.Base;

namespace ProjectName.Models.PlayList
{
    public class DashboardPlayListDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.PlayList>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardPlayListShareViewModel>(parameters);
        }
    }
}
