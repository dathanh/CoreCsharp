namespace ProjectName.Models.Base
{
    public class ViewColumnViewModel
    {
        public ViewColumnViewModel()
        {
            Sortable = true;
            Filterable = false;
        }
        /// <summary>
        /// Column header text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Column databind
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Column width
        /// </summary>
        public int ColumnWidth { get; set; }
        /// <summary>
        /// Column justificate (Left, right)
        /// </summary>
        public GridColumnJustification ColumnJustification { get; set; }
        /// <summary>
        /// Column format( format with number, text, decimal...)
        /// </summary>
        public string ColumnFormat { get; set; }
        /// <summary>
        /// Column format date UTC
        /// </summary>
        public bool? UtcFormat { get; set; }
        /// <summary>
        /// Hide or show column
        /// </summary>
        public bool HideColumn { get; set; }
        /// <summary>
        /// Position column in grid
        /// </summary>
        public int ColumnOrder { get; set; }
        /// <summary>
        /// This is madatory column or not( user cannot move, resize, show/hide)
        /// </summary>
        public bool Mandatory { get; set; }

        public bool Sortable { get; set; }

        public bool Filterable { get; set; }

        public string CustomTemplate { get; set; }

    }

    public enum GridColumnJustification
    {
        Left,
        Right,
        Center
    }
}