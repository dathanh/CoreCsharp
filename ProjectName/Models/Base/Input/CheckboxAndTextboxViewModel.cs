using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Input
{
    public class CheckboxAndTextboxViewModel : ControlSharedViewModelBase
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string NgChangeFunction { get; set; }

        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public string TextboxId { get; set; }
        public int? TextboxLength { get; set; }
        public string TextboxClass { get; set; }
        public string TextboxDataBindingValue { get; set; }
    }
}