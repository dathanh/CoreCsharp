using System.Collections.Generic;

namespace ProjectName.Models.Base
{
    public class GridConfigViewModel
    {
        public int Id { get; set; }

        public int DocumentTypeId { get; set; }

        public int? UserId { get; set; }

        public string GridInternalName { get; set; }

        public bool AllowResizeColumn { get; set; }

        public bool AllowReorderColumn { get; set; }

        public bool AllowShowHideColumn { get; set; }

        public List<ViewColumnViewModel> ViewColumns { get; set; }
    }
}