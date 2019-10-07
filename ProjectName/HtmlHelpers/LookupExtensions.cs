using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using ProjectName.Models.Base;

namespace ProjectName.HtmlHelpers
{
    public static class LookupExtensions
    {
        public static IHtmlContent Lookup(this IHtmlHelper htmlHelper, string lookupId,
           string label,
           string modelName,
           string dataBindingValue, string dataBindingText = "",
           string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12",
           string url = "",
           bool populatedByChildren = false,
           string urlToReadData = "", string urlToGetLookupItem = "", bool isRequired = false, int currentId = 0, string placeHolderText = "", object htmlAttribute = null, int heightLookup = 250,
           string customParams = "",
           string isShow = "true",
          string addLookupPopupFunction = "",
           string editLookupPopupFunction = "",
            string advancedSearchLookupPopupFunction = "",
           string moreClass = "",
           bool showAddEdit = false,
            bool showAdvancedSearch = false,
           bool enable = true,
            string hierarchyKeyToSend = "", string hierarchyKeyToListen = null,
            string popupAddEditId = "", string popupAddEditInstalFunction = "", string popupAddEditOptions = "",
            string popupAdvancedSearchId = "", string popupAdvancedSearchInstalFunction = "", string popupAdvancedSearchOptions = "",
            string parameterDependencies = "", bool isAutoBind = false, bool isNonResetValueWhenSelect = false)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", lookupId);

            if (attribute.ContainsKey("style"))
            {
                if (!attribute["style"].ToString().Contains("width"))
                {
                    attribute["style"] = attribute["style"] + "width: 270px;";
                }
            }
            else
            {
                attribute.Add("style", "");
            }

            if (isRequired)
            {
                attribute["required"] = "required";
            }

            attribute["style"] = attribute["style"] + "display: none;";

            var model = new LookupViewModel
            {
                Id = lookupId,
                CurrentId = currentId,
                Label = label,
                ModelName = modelName,
                UrlToReadData = urlToReadData,
                HtmlAttributes = attribute,
                PopulatedByChildren = populatedByChildren,
                HeightLookup = heightLookup,
                DataBindingValue = dataBindingValue,
                DataBindingText = dataBindingText,
                Required = isRequired,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                CustomParams = customParams,
                IsShow = isShow,
                AddLookupPopupFunction = addLookupPopupFunction,
                EditLookupPopupFunction = editLookupPopupFunction,
                AdvancedSearchLookupPopupFunction = advancedSearchLookupPopupFunction,
                MoreClass = moreClass,
                ShowAddEdit = showAddEdit,
                ShowAdvancedSearch = showAdvancedSearch,
                Enabled = enable,
                PlaceHolderText = placeHolderText,
                HierarchyKeyToSend = hierarchyKeyToSend,
                HierarchyKeyToListen = hierarchyKeyToListen,
                PopupAddEditId = popupAddEditId,
                PopupAddEditInstalFunction = popupAddEditInstalFunction,
                PopupAddEditOptions = popupAddEditOptions,
                PopupAdvancedSearchId = popupAdvancedSearchId,
                PopupAdvancedSearchInstalFunction = popupAdvancedSearchInstalFunction,
                PopupAdvancedSearchOptions = popupAdvancedSearchOptions,
                ParameterDependencies = parameterDependencies,
                IsAutoBind = isAutoBind,
                IsNonResetValueWhenSelect = isNonResetValueWhenSelect
            };

            if (string.IsNullOrWhiteSpace(url))
            {
                url = "~/Views/Shared/Lookup/_Lookup.cshtml";
            }
            var partialTask = htmlHelper.PartialAsync(url, model);
            return partialTask.Result;
        }

        public static IHtmlContent LookupLabelInLine(this IHtmlHelper htmlHelper, string lookupId,
           string label,
           string modelName,
           string dataBindingValue,
           string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12",
           string url = "",
           bool populatedByChildren = false,
           string urlToReadData = "", string urlToGetLookupItem = "", bool isRequired = false, int currentId = 0, string placeHolderText = "", object htmlAttribute = null, int heightLookup = 250,
           string customParams = "",
           string isShow = "true",
          string addLookupPopupFunction = "",
           string editLookupPopupFunction = "",
            string advancedSearchLookupPopupFunction = "",
           string moreClass = "",
           bool showAddEdit = false,
            bool showAdvancedSearch = false,
           bool enable = true,
            string hierarchyKeyToSend = "", string hierarchyKeyToListen = null,
            string popupAddEditId = "", string popupAddEditInstalFunction = "", string popupAddEditOptions = "",
            string popupAdvancedSearchId = "", string popupAdvancedSearchInstalFunction = "", string popupAdvancedSearchOptions = "", string parameterDependencies = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", lookupId);

            if (attribute.ContainsKey("style"))
            {
                if (!attribute["style"].ToString().Contains("width"))
                {
                    attribute["style"] = attribute["style"] + "width: 270px;";
                }
            }
            else
            {
                attribute.Add("style", "");
            }

            if (isRequired)
            {
                attribute["required"] = "required";
            }

            attribute["style"] = attribute["style"] + "display: none;";

            var model = new LookupViewModel
            {
                Id = lookupId,
                CurrentId = currentId,
                Label = label,
                ModelName = modelName,
                UrlToReadData = urlToReadData,
                HtmlAttributes = attribute,
                PopulatedByChildren = populatedByChildren,
                HeightLookup = heightLookup,
                DataBindingValue = dataBindingValue,
                Required = isRequired,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                CustomParams = customParams,
                IsShow = isShow,
                AddLookupPopupFunction = addLookupPopupFunction,
                EditLookupPopupFunction = editLookupPopupFunction,
                AdvancedSearchLookupPopupFunction = advancedSearchLookupPopupFunction,
                MoreClass = moreClass,
                ShowAddEdit = showAddEdit,
                ShowAdvancedSearch = showAdvancedSearch,
                Enabled = enable,
                PlaceHolderText = placeHolderText,
                HierarchyKeyToSend = hierarchyKeyToSend,
                HierarchyKeyToListen = hierarchyKeyToListen,
                PopupAddEditId = popupAddEditId,
                PopupAddEditInstalFunction = popupAddEditInstalFunction,
                PopupAddEditOptions = popupAddEditOptions,
                PopupAdvancedSearchId = popupAdvancedSearchId,
                PopupAdvancedSearchInstalFunction = popupAdvancedSearchInstalFunction,
                PopupAdvancedSearchOptions = popupAdvancedSearchOptions,
                ParameterDependencies = parameterDependencies
            };

            if (string.IsNullOrWhiteSpace(url))
            {
                url = "~/Views/Shared/Lookup/_LookupLabelInLine.cshtml";
            }
            var partialTask = htmlHelper.PartialAsync(url, model);
            return partialTask.Result;
        }

    }
}
