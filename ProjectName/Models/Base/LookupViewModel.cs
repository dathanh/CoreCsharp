using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace ProjectName.Models.Base
{
    public class LookupViewModel : ControlSharedViewModelBase
    {
        public string ModelName { get; set; }
        public string UrlToReadData { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public bool PopulatedByChildren { get; set; }
        public List<object> DataSource { get; set; }
        public int HeightLookup { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";

        public string CustomParams { get; set; }

        public string IsShow { get; set; }

        public string AddLookupPopupFunction { get; set; }
        public string EditLookupPopupFunction { get; set; }
        public string AdvancedSearchLookupPopupFunction { get; set; }
        public string MoreClass { get; set; }
        public bool ShowAddEdit { get; set; }
        public bool ShowAdvancedSearch { get; set; }
        public string PlaceHolderText { get; set; }
        public int CurrentId { get; set; }
        public string HierarchyKeyToListen { get; set; }
        public string HierarchyKeyToSend { get; set; }
        public string PopupAddEditId { get; set; }
        public string PopupAddEditInstalFunction { get; set; }
        public string PopupAddEditOptions { get; set; }
        public string PopupAdvancedSearchId { get; set; }
        public string PopupAdvancedSearchInstalFunction { get; set; }
        public string PopupAdvancedSearchOptions { get; set; }
        public string ParameterDependencies { get; set; }
        public bool IsAutoBind { get; set; }
        public string DataBindingText { get; set; }
        public bool IsNonResetValueWhenSelect { get; set; }
    }

    public class HierarchyGroup
    {
        public string HierarchyKeyToListen { get; set; }
        public string HierarchyFieldName { get; set; }
    }
}