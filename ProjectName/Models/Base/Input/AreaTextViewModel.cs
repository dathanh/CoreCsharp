using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class AreaTextViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string PlaceHolderText { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public int Cols { get; set; }
        public int Rows { get; set; }
        public double WidthPercentLable { get; set; }
        public double WidthPercentField { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public int MaxLength { get; set; }
        public int Height { get; set; }
        public string HeightStr => Height + "px";
        public string MoreClass { get; set; }
        public string OnChange { get; set; }
        public bool IsExcludeFormModified { get; set; }
        public bool AutoHeight { get; set; }
    }
}