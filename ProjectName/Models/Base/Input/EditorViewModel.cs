using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class EditorViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public RouteValueDictionary HtmlAttributes { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public string UrlRead { get; set; }
        public string UrlDestroy { get; set; }
        public string UrlCreate { get; set; }
        public string UrlThumb { get; set; }
        public string UrlUpload { get; set; }
        public string UrlImage { get; set; }
        public bool IsDisabled { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
        public bool IsBasic { get; set; }
    }
}