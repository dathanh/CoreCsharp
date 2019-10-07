using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class InputTextViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string PlaceHolderText { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string TextboxType { get; set; }
        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public string ButtonFunctionName { get; set; }
        public string ButtonFunctionText { get; set; }
        public string AutoComplete { get; set; }
        public bool IsNumberOnly { get; set; }
        public bool IsShowInfoPasswordRule { get; set; }
        public string DataPlacementTooltipInfoPasswordRule { get; set; }

        public bool IsNumberAndCharacterOnly { get; set; }
        public bool IsNotAllowWhiteSpace { get; set; }
        public string CallbackFunction { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
    }
}