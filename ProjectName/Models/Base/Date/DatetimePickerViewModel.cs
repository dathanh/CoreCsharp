using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Date
{
    public class DateTimePickerViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public string Format { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public RouteValueDictionary HtmlAttributes { get; set; }
        public bool HasTime { get; set; }
        public bool HasMin { get; set; }
        public string PlaceHolderText { get; set; }
        public string MoreClass { get; set; }
        public bool ShowCurrentDateCheckbox { get; set; }
        public string CurrentDateModel { get; set; }
    }
}