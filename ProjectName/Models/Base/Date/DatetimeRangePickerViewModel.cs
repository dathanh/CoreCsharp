using Microsoft.AspNetCore.Routing;

namespace ProjectName.Models.Base.Date
{
    public class DatetimeRangePickerViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Format { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public string IdStart { get; set; }
        public string LabelStart { get; set; }
        public RouteValueDictionary HtmlAttributesStart { get; set; }
        public string StyleStart { get; set; }
        public string DataBindingValueStart { get; set; }
        public bool HasTime { get; set; }
        public string IdEnd { get; set; }
        public string LabelEnd { get; set; }
        public string StyleEnd { get; set; }
        public RouteValueDictionary HtmlAttributesEnd { get; set; }
        public string DataBindingValueEnd { get; set; }
    }
}