using System.Collections.Generic;

namespace ProjectName.Models.Base.Date
{
    public class DropDownListViewModel : ControlSharedViewModelBase
    {
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public string ReadUrl { get; set; }

        public string MoreClass { get; set; }

        public string PlaceHolderText { get; set; }
        public string PlaceHolderValue { get; set; }

        public bool IsShowSelectAll { get; set; }
        public bool IsAutoBind { get; set; }
        public bool IsDisplayOnTop { get; set; }
        public string OnchangeFunction { get; set; }
        public string ParameterDependencies { get; set; }
        public string OnChangeDatasourceFunction { get; set; }
        public string OnDataBoundFunction { get; set; }
        public string DataBindingText { get; set; }
        public bool SetFirstValueIsDefault { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
        public List<string> CustomParams { get; set; }
        public bool IsJustAutoComplete { get; set; }
    }
}