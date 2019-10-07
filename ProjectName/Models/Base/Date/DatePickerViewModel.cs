using Microsoft.AspNetCore.Routing;
using System;

namespace ProjectName.Models.Base.Date
{
    public class DatePickerViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public string Format { get; set; }
        public bool Required { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string PlaceHolderText { get; set; }
        public bool IsMaked { get; set; }
    }
}