using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class InputMaskedViewModel : ControlSharedViewModelBase
    {
        public string Format { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";

        public string PlaceHolderText { get; set; }
        public string MoreClass { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
    }
}