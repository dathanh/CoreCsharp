using System.Collections.Generic;

namespace ProjectName.Models.Base
{
    public class ExportExcel
    {
        public List<ColumnModel> GridConfigViewModel { get; set; }
        public List<dynamic> ListDataSource { get; set; }
    }
}