using ProjectName.Models.Base;

namespace ProjectName.Models.Video
{
    public class DashboardVideoDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Video>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardVideoShareViewModel>(parameters);
        }
    }
}