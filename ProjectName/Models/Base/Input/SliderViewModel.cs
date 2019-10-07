using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class SliderViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public RouteValueDictionary HtmlAttributes { get; set; }

        public string Orientation { get; set; }
        public bool IsDisabled { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}