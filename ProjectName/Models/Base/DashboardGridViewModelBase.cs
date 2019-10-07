using Framework.DomainModel;
using Newtonsoft.Json;

namespace ProjectName.Models.Base
{
    public class DashboardGridViewModelBase<TEntity> : ViewModelBase
                where TEntity : Entity
    {
        public override string PageTitle => typeof(TEntity).Name;

        private GridViewModel _gridViewModel;
        /// <summary>
        /// Gets or sets the grid view model.
        /// </summary>
        [JsonIgnore]
        public virtual GridViewModel GridViewModel
        {
            get => _gridViewModel;
            set => _gridViewModel = value;//var entityName = typeof(TEntity).Name;//if (string.IsNullOrWhiteSpace(_gridViewModel.ModelName))//{//    _gridViewModel.ModelName = entityName;//    _gridViewModel.GridInternalName = entityName;//}//if (_gridViewModel.DocumentTypeId == 0)//{//    DocumentTypeKey documentType;//    Enum.TryParse(entityName, out documentType);//    _gridViewModel.DocumentTypeId = (int)documentType;//}//_gridViewModel.PageTitle = PageTitle;//_gridViewModel.CurrentUser = DependencyResolver.Current.GetService<IAuthenticationService>().GetCurrentUser();
        }
    }
}