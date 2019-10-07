using ProjectName.Models.Base;

namespace ProjectName.Models.Video
{
    public class DashboardVideoIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Video>
    {
        public bool CanAddNewRecord { get; set; }
    }
}