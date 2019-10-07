using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class CheckBoxViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string NgChangeFunction { get; set; }

        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public bool LabelTop { get; set; }
        public bool HideLable { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
    }
}