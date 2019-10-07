using ProjectName.Models.Base;

namespace ProjectName.Models.Banner
{
    public class DashboardBannerIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Banner>
    {
        public bool CanAddNewRecord { get; set; }
    }
}