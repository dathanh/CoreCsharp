using ProjectName.Models.Base;

namespace ProjectName.Models.Category
{
    public class DashboardCategoryDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Category>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardCategoryShareViewModel>(parameters);
        }
    }
}