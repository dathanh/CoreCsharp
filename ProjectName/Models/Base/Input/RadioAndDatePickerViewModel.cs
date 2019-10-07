using Microsoft.AspNetCore.Routing;
using System;

namespace ProjectName.Models.Base.Input
{
    public class RadioAndDatePickerViewModel : ControlSharedViewModelBase
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string NgChangeFunction { get; set; }

        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public string Value { get; set; }
        public string DateId { get; set; }
        public string DateClass { get; set; }
        public string DateDataBindingValue { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string PlaceHolderText { get; set; }
        public string Format { get; set; }

    }
}