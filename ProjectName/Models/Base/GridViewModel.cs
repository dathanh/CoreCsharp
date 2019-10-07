using System.Collections.Generic;

namespace ProjectName.Models.Base
{
    /// <summary>
    /// Grid view model
    /// </summary>
    public class GridViewModel : ViewModelBase
    {
        public GridViewModel()
        {
            CanAddNewRecord = true;
            CanDeleteRecord = true;
            CanUpdateRecord = true;
            CustomParameters = new List<string>();
            NumberLineHeader = 1;
        }
        /// <summary>
        /// Grid id to distinguish grid in page
        /// </summary>
        public string GridId { get; set; }
        /// <summary>
        /// Model name to get data
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// Grid internal name( use in grid in grid)
        /// </summary>
        public string GridInternalName { get; set; }
        /// <summary>
        /// Advance search url
        /// </summary>
        public string AdvancedSearchUrl { get; set; }
        /// <summary>
        /// Include/Exclude active item in grid
        /// </summary>
        public bool ExcludeFilterActiveRecords { get; set; }
        /// <summary>
        /// List column in grid
        /// </summary>
        public IList<ViewColumnViewModel> ViewColumns { get; set; }

        public string UserCustom { get; set; }

        public int? ActionDefaultWidthColumn { get; set; }
        public bool UseActionDefaultColumn { get; set; }

        public bool CanAddNewRecord { get; set; }

        public bool CanExportExcel { get; set; }

        public bool CanUpdateRecord { get; set; }

        public bool CanDeleteRecord { get; set; }

        public bool? IsLazyPaging { get; set; }

        public int PopupWidth { get; set; }

        public int PopupHeight { get; set; }

        public string AddFunction { get; set; }
        public string UpdateFunction { get; set; }
        public string CancelFunction { get; set; }
        public string DeleteFunction { get; set; }
        public string ExtFunc1 { get; set; }
        public string ExtFunc2 { get; set; }
        public string ExtFunc3 { get; set; }
        public string ExtFunc4 { get; set; }
        public string ExtFunc5 { get; set; }
        public string ParentSearch { get; set; }
        public string DataBoundFunc { get; set; }
        public string DetailTemplate { get; set; }
        public string DetailInitBind { get; set; }
        public string CustomHeaderTemplate { get; set; }

        public List<string> CustomParameters { get; set; }

        public bool DisableAutoBind { get; set; }
        public string SearchPlaceholder { get; set; }
        public int NumberLineHeader { get; set; }
    }

}