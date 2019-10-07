using System.Collections.Generic;

namespace ProjectName.Models.Base
{
    public class ExportExcelModel
    {
        public List<ColumnModel> Columns { get; set; }

        public List<dynamic> DataSource { get; set; }
    }

    public class ColumnModel
    {
        public string Field { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}