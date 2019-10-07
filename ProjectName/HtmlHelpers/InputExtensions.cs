using Framework.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using ProjectName.Models.Base.Date;
using ProjectName.Models.Base.Input;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProjectName.HtmlHelpers
{
    public static class InputExtensions
    {
        public static IHtmlContent GenericDualListBox(this IHtmlHelper htmlHelper, string modelName, int entityId, string avaliableAction, string selectedAction,
                                                                                                           string queryEntityName, string headerText = "")
        {
            var model = new DualListBoxViewModel
            {
                ControlId = modelName,
                ModelName = modelName,
                GetAllUrl = string.Format("/{0}/{1}", modelName, avaliableAction),
                GetSelectedUrl = string.Format("/{0}/{1}?{2}= {3}", modelName, selectedAction, queryEntityName, entityId),
                HeaderText = headerText

            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/DualListBox/_DualListBoxViewModel.cshtml", model);
            return partialTask.Result;
        }

        public static IHtmlContent CustomTextBoxWithButton(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false, int length = Int32.MaxValue,
            bool isReadonly = false, string cssClass = "", object htmlAttribute = null, string placeHolderText = "", string buttonFunctionName = "", string buttonFunctionText = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new InputTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                Length = length,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                TextboxType = "type=text",
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                ButtonFunctionName = buttonFunctionName,
                ButtonFunctionText = buttonFunctionText

            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/InputTextWithButton.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomTextBox(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false, int length = Int32.MaxValue,
            bool isReadonly = false, string cssClass = "k-textbox", object htmlAttribute = null,
            string placeHolderText = "", bool isPasswordType = false, bool isDisabled = false, string moreClass = "",
            string autoComplete = "on", bool isNumberOnly = false, bool isNumberAndCharacterOnly = false, bool isNotAllowWhiteSpace = false,
            bool isShowInfoPasswordRule = false, string dataPlacementTooltipInfoPasswordRule = "bottom", string requireMarkColor = "red", string requireMarkPosition = "left")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new InputTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                Length = length,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                TextboxType = isPasswordType ? "type=password" : "type=text",
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                AutoComplete = autoComplete,
                IsNumberOnly = isNumberOnly,
                IsShowInfoPasswordRule = isShowInfoPasswordRule,
                DataPlacementTooltipInfoPasswordRule = dataPlacementTooltipInfoPasswordRule,

                IsNumberAndCharacterOnly = isNumberAndCharacterOnly,
                IsNotAllowWhiteSpace = isNotAllowWhiteSpace,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition,

            };
            if (isPasswordType)
            {
                //viewModel.Class += " k-textbox";
            }
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/InputText.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomSlider(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false,
             string orientation = "", object htmlAttribute = null, bool isDisabled = false, string width = "100%", string height = "100%",
            int min = 0, int max = 100)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new SliderViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                DataBindingValue = dataBindingValue,
                IsDisabled = isDisabled,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Orientation = orientation,
                Width = width,
                Height = height,
                Min = min,
                Max = max
            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/Slider.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomAddressAutoComplete(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false, int length = Int32.MaxValue,
            bool isReadonly = false, string cssClass = "k-textbox", object htmlAttribute = null,
            string placeHolderText = "", string moreClass = "", string callbackFunction = "onPlaceChanged()")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new InputTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                Length = length,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                MoreClass = moreClass,
                CallbackFunction = callbackFunction
            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/AddressAutoComplete.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomCheckBox(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isReadonly = false, string cssClass = "k-checkbox",
            object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false, string moreClass = "", bool labelTop = false, bool hideLable = false
            )
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new CheckBoxViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                LabelTop = labelTop,
                HideLable = hideLable,

            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/CheckBox.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomRadio(this IHtmlHelper htmlHelper, string id, string name, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isReadonly = false, string cssClass = "k-radio",
            object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false, string value = "", string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new RadioViewModel
            {
                Id = id,
                Name = name,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                Value = value
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/Radio.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomRadioAndTextbox(this IHtmlHelper htmlHelper, string id, string name, string label, string dataBindingValue, int? textboxLength,
            string textboxDataBindingValue, object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false,
            string value = "", string moreClass = "", string textboxId = "", string cssClass = "k-radio", bool isReadonly = false, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12"
            , string textboxClass = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new RadioAndTextboxViewModel
            {
                Id = id,
                Name = name,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                Value = value,
                TextboxId = textboxId,
                TextboxLength = textboxLength,
                TextboxClass = textboxClass,
                TextboxDataBindingValue = textboxDataBindingValue
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/RadioAndTextbox.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomCheckboxAndTextbox(this IHtmlHelper htmlHelper, string id, string name, string label, string dataBindingValue, int? textboxLength,
            string textboxDataBindingValue, object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false
            , string moreClass = "", string textboxId = "", string cssClass = "k-checkbox", bool isReadonly = false, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12"
            , string textboxClass = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new CheckboxAndTextboxViewModel
            {
                Id = id,
                Name = name,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                TextboxId = textboxId,
                TextboxLength = textboxLength,
                TextboxClass = textboxClass,
                TextboxDataBindingValue = textboxDataBindingValue
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/CheckboxAndTextbox.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomRadioAndDatePicker(this IHtmlHelper htmlHelper, string id, string name, string label, string dataBindingValue,
            string dateDataBindingValue, DateTime? minDate = null, DateTime? maxDate = null, string placeHolderText = "", object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false,
            string value = "", string moreClass = "", string dateId = "", string cssClass = "k-radio", bool isReadonly = false, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12"
            , string dateClass = "", string format = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new RadioAndDatePickerViewModel
            {
                Id = id,
                Name = name,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                Value = value,
                DateId = dateId,
                MinDate = minDate,
                MaxDate = maxDate,
                PlaceHolderText = placeHolderText,
                DateClass = dateClass,
                DateDataBindingValue = dateDataBindingValue,
                Format = format
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/RadioAndDatePicker.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomCheckBoxAndDatePicker(this IHtmlHelper htmlHelper, string id, string name, string label, string dataBindingValue,
            string dateDataBindingValue, DateTime? minDate = null, DateTime? maxDate = null, string placeHolderText = "", object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false,
            string value = "", string moreClass = "", string dateId = "", string cssClass = "k-radio", bool isReadonly = false, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12"
            , string dateClass = "", string format = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new CheckBoxAndDatePickerViewModel
            {
                Id = id,
                Name = name,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                Value = value,
                DateId = dateId,
                MinDate = minDate,
                MaxDate = maxDate,
                PlaceHolderText = placeHolderText,
                DateClass = dateClass,
                DateDataBindingValue = dateDataBindingValue,
                Format = format
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/CheckBoxAndDatePicker.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomTextArea(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false, bool isReadonly = false,
            string cssClass = "k-textbox", object htmlAttribute = null, string placeHolderText = "", int cols = 2,
            int rows = 2, double widthPercentLable = 14, double widthPercentField = 80, int maxlength = Int32.MaxValue,
            int height = 100, string moreClass = "", string onChange = "", bool isExcludeFormModified = false, bool autoHeight = false)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new AreaTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                Cols = cols,
                Rows = rows,
                WidthPercentLable = widthPercentLable,
                WidthPercentField = widthPercentField,
                MaxLength = maxlength,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Height = height,
                MoreClass = moreClass,
                OnChange = onChange,
                IsExcludeFormModified = isExcludeFormModified,
                AutoHeight = autoHeight
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/TextArea.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomDatePicker(this IHtmlHelper htmlHelper, string id, string label, string dataBindingValue, string format,
            bool required = false, bool isReadonly = false, string cssClass = "k-datepicker", object htmlAttribute = null, DateTime? minDate = null,
            DateTime? maxDate = null, string placeHolderText = null, string classcol = "col-md-3 col-sm-3 col-xs-12",
            string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isMaked = true)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);
            var viewModel = new DatePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                MinDate = minDate,
                MaxDate = maxDate,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                PlaceHolderText = placeHolderText,
                IsMaked = isMaked
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/DatePicker.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomDatetimePicker(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string format = "MM/dd/yyyy hh:mm tt", bool isReadonly = false, string cssClass = "k-datetimepicker",
            object htmlAttribute = null, string placeHolderText = null, string moreClass = "", bool required = false, bool showCurrentDateCheckbox = false, string currentDateModel = null)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new DateTimePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                PlaceHolderText = placeHolderText,
                MoreClass = moreClass,
                Required = required,
                ShowCurrentDateCheckbox = showCurrentDateCheckbox,
                CurrentDateModel = currentDateModel
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/DateTimePicker.cshtml", viewModel);
            return partialTask.Result;
        }
        public static IHtmlContent CustomTimePicker(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string format = "hh:mm tt",
            bool required = false, bool isReadonly = false, string cssClass = "k-timepicker",
            object htmlAttribute = null, bool hasTime = false, bool hasMin = false, string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new DateTimePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                HasTime = hasTime,
                HasMin = hasMin,
                MoreClass = moreClass,
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/TimePicker.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomDatetimeRangePicker(this IHtmlHelper htmlHelper,
            string idStart, string labelStart, string dataBindingValueStart,
            string idEnd, string labelEnd, string dataBindingValueEnd, string format,
            bool hasTime = false, object htmlAttributeEnd = null, object htmlAttributeStart = null,
            bool required = false, bool isReadonly = false, string cssClass = "", string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12")
        {
            var attributeStart = new RouteValueDictionary();
            //if (htmlAttributeStart != null)
            //{
            //    attributeStart = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributeStart);
            //}

            attributeStart.Add("id", idStart);

            var attributeEnd = new RouteValueDictionary();
            //if (htmlAttributeEnd != null)
            //{
            //    attributeEnd = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributeEnd);
            //}

            attributeEnd.Add("id", idEnd);

            var viewModel = new DatetimeRangePickerViewModel
            {
                Class = cssClass,
                ReadOnly = isReadonly,
                Required = required,
                Format = format,

                IdStart = idStart,
                LabelStart = labelStart,
                HtmlAttributesStart = attributeStart,
                DataBindingValueStart = dataBindingValueStart,
                HasTime = hasTime,

                IdEnd = idEnd,
                LabelEnd = labelEnd,
                DataBindingValueEnd = dataBindingValueEnd,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/DateTimeRangePicker.cshtml", viewModel);
            return partialTask.Result;
        }

        private static IHtmlContent InputNumericBase(this IHtmlHelper htmlHelper, string id, string label, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12",
            int length = 10, string dataBindingValue = "", int width = 200,
            string format = "", object minimumValue = null,
            object maximumValue = null, double stepValue = 1, bool readOnly = false, int decimals = 2,
            string placeHolderText = "", bool isRequired = false)
        {
            var viewModel = new InputNumericViewModel
            {
                Id = id,
                Label = label,
                Length = length,
                DataBindingValue = string.IsNullOrWhiteSpace(dataBindingValue) ? "''" : dataBindingValue,
                Width = width,
                Format = format,
                MinimumValue = minimumValue,
                MaximumValue = maximumValue,
                StepValue = stepValue,
                PlaceHolderText = placeHolderText,
                ReadOnly = readOnly,
                Decimals = decimals,
                Required = isRequired,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield

            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/InputNumeric.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent InputPositiveNumeric(this IHtmlHelper htmlHelper, string id, string label,
                   string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12"
                    , int width = 200, string format = "", int maximumValue = 999999999,
            bool readOnly = false, bool isRequired = false, int stepValue = 1)
        {
            var length = maximumValue.ToString(CultureInfo.InvariantCulture).Length;
            return InputNumericBase(htmlHelper, id, label, classcol, classlabel, classfield, length, dataBindingValue, width, format,
                0, maximumValue, stepValue, readOnly, 0, "", isRequired);
        }

        public static IHtmlContent InputNumeric(this IHtmlHelper htmlHelper, string id, string label, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", int length = 10,
            string dataBindingValue = "", int width = 200, string format = "",
            double minimumValue = 0.1,
            double maximumValue = 999999999, int stepValue = 1, bool readOnly = false, int decimals = 2,
            string placeHolderText = "", bool isRequired = false)
        {
            var decimalLength = decimals > 0 ? decimals + 1 : 0;
            var maxlength = maximumValue.ToString(CultureInfo.InvariantCulture).Length + decimalLength;
            if (decimals > 0 && string.IsNullOrWhiteSpace(format))
            {
                format = string.Format("#,#.{0}", "0".PadRight(decimals, '0'));
            }
            return InputNumericBase(htmlHelper, id, label, classcol, classlabel, classfield, maxlength, dataBindingValue, width,
                format, minimumValue,
                maximumValue, stepValue, readOnly, decimals, placeHolderText, isRequired);

        }

        public static IHtmlContent CustomZipcode(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string placeHolderText = "", bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, classcol, classlabel, classfield, "00000", isRequired,
                readOnly,
                htmlAttribute, placeHolderText);
        }

        public static IHtmlContent CustomPhone(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string placeHolderText = "", bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null, string requireMarkColor = "red", string requireMarkPosition = "left")
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, classcol, classlabel, classfield, "(999) 000-0000", isRequired,
                readOnly,
                htmlAttribute, placeHolderText, requireMarkColor: requireMarkColor, requireMarkPosition: requireMarkPosition);

        }

        public static IHtmlContent CustomSocialSecurityNumber(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null, string placeHolderText = null, string moreClass = "")
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, classcol, classlabel, classfield, "000-00-0000", isRequired,
                readOnly,
                htmlAttribute, placeHolderText, moreClass);
        }

        public static IHtmlContent CustomNPI(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, classcol, classlabel, classfield, "0000000000", isRequired,
                readOnly,
                htmlAttribute);
        }

        public static IHtmlContent CustomReqNumPrefix(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, classcol, classlabel, classfield, "LLL", isRequired,
                readOnly,
                htmlAttribute);
        }


        public static IHtmlContent InputMasked(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol, string classlabel, string classfield, string format, bool isRequired = false,
            bool readOnly = false,
            object htmlAttribute = null, string placeHolderText = null, string moreClass = "", string requireMarkColor = "red", string requireMarkPosition = "left")
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new InputMaskedViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = isRequired,
                DataBindingValue = dataBindingValue,
                Format = format,
                ReadOnly = readOnly,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                PlaceHolderText = placeHolderText,
                MoreClass = moreClass,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/InputMasked.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomEditor(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", bool required = false, int width = 50, int height = 50,
            string urlRead = null,
            string urlDestroy = null, string urlCreate = null, string urlThumb = null, string urlUpload = null,
            string urlImage = null, bool isReadonly = false, bool isDisabled = false, string cssClass = "k-textbox", object htmlAttribute = null, string requireMarkColor = "red", string requireMarkPosition = "left", bool isBasic = false)
        {
            var attribute = new RouteValueDictionary();
            //if (htmlAttribute != null)
            //{
            //    attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            //}

            attribute.Add("id", id);

            var viewModel = new EditorViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Width = width,
                Height = height,
                UrlRead = urlRead,
                UrlDestroy = urlDestroy,
                UrlCreate = urlCreate,
                UrlThumb = urlThumb,
                UrlUpload = urlUpload,
                UrlImage = urlImage,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                IsDisabled = isDisabled,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition,
                IsBasic = isBasic
            };

            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/Editor.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent InputWithAttributes(this IHtmlHelper helper, string id, object htmlAttributes = null)
        {
            var control = new TagBuilder("input");

            foreach (var attribute in (RouteValueDictionary)htmlAttributes)
            {
                control.MergeAttribute(attribute.Key, attribute.Value.ToString());
            }

            return control;
        }

        public static IHtmlContent CustomDropDownList(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string readUrl = "", bool required = false,
            bool enable = true, string moreClass = "", string onchangeFunction = "", string onDataBoundFuc = "", bool isAutoBind = false,
            string placeHolderText = "", string dataBindingText = "", string placeHolderValue = "0", bool setFirstValueIsDefault = false, string requireMarkColor = "red", string requireMarkPosition = "left", string parameterDependencies = "",
            List<string> customParams = null)
        {
            var viewModel = new DropDownListViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Required = required,
                ReadUrl = readUrl,
                MoreClass = moreClass,
                Enabled = enable,
                OnchangeFunction = onchangeFunction,
                OnDataBoundFunction = onDataBoundFuc,
                IsAutoBind = isAutoBind,
                PlaceHolderText = placeHolderText,
                DataBindingText = dataBindingText,
                PlaceHolderValue = placeHolderValue,
                SetFirstValueIsDefault = setFirstValueIsDefault,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition,
                ParameterDependencies = parameterDependencies,
                CustomParams = customParams

            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/DropDownList.cshtml", viewModel);
            return partialTask.Result;
        }


        public static IHtmlContent CustomComboBoxAutoComplete(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string readUrl = "", bool required = false,
            bool enable = true, string moreClass = "", string onchangeFunction = "", string parameterDependencies = "", bool isReadOnly = false, bool isAutoBind = false,
            string placeHolderText = "", string dataBindingText = "")
        {
            var viewModel = new DropDownListViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Required = required,
                ReadUrl = readUrl,
                MoreClass = moreClass,
                Enabled = enable,
                OnchangeFunction = onchangeFunction,
                ParameterDependencies = parameterDependencies,
                ReadOnly = isReadOnly,
                IsAutoBind = isAutoBind,
                PlaceHolderText = placeHolderText,
                DataBindingText = dataBindingText
            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/ComboBoxAutoComplete.cshtml", viewModel);
            return partialTask.Result;
        }
        public static IHtmlContent CustomAutoComplete(this IHtmlHelper htmlHelper, string id, string label,
        string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string readUrl = "",
            bool required = false, string moreClass = "", string parameterDependencies = "", int maxlength = Int32.MaxValue, bool isReadonly = false,
            string onchangeFunction = "", string requireMarkColor = "red", string requireMarkPosition = "left")
        {
            var viewModel = new AutoCompleteViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Required = required,
                ReadUrl = readUrl,
                MoreClass = moreClass,
                ParameterDependencies = parameterDependencies,
                Length = maxlength,
                ReadOnly = isReadonly,
                OnchangeFunction = onchangeFunction,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition

            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/AutoComplete.cshtml", viewModel);
            return partialTask.Result;
        }

        public static IHtmlContent CustomMultiSelect(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string readUrl = "", bool required = false,
            bool enable = true, string moreClass = "", string parameterDependencies = "", bool isShowSelectAll = false, string placeHolder = "",
            string onchangeDatasourceFunc = "", bool isDisplayOnTop = false, string dataBindingText = "", string requireMarkColor = "red", string requireMarkPosition = "left")
        {
            var viewModel = new DropDownListViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Required = required,
                ReadUrl = readUrl,
                MoreClass = moreClass,
                ParameterDependencies = parameterDependencies,
                Enabled = enable,
                IsShowSelectAll = isShowSelectAll,
                PlaceHolderText = placeHolder,
                OnChangeDatasourceFunction = onchangeDatasourceFunc,
                IsDisplayOnTop = isDisplayOnTop,
                DataBindingText = dataBindingText,
                RequireMarkColor = requireMarkColor,
                RequireMarkPosition = requireMarkPosition
            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/MultiSelect.cshtml", viewModel);
            return partialTask.Result;
        }


        public static IHtmlContent CustomFileUploadAngular(this IHtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, bool isMultiple = false, string classcol = "col-md-3 col-sm-3 col-xs-12", string classlabel = "col-md-12 col-sm-12 col-xs-12", string classfield = "col-md-12 col-sm-12 col-xs-12", string saveUrl = "/Common/SaveFileUpload", string removeUrl = "/Common/RemoveFileUpload",
            int previewWidth = 32, int previewHeight = 32, bool required = false, string acceptType = "*", bool isUploadImage = false, bool isChangeAvatar = false, string selectText = "Select files", bool isAllowMultiFile = false, bool isReturnObject = false, string onsuccessFunction = "", int imageType = 0, bool isFakeRemove = false, bool isUploadVideo = false)
        {
            //var url = ImageHelper.GetUrlCdn();
            var url ="";
            if (saveUrl == "/Common/SaveFileUpload")
            {
                if (imageType != 0)
                {
                    saveUrl = url + "/Common/SaveFileUploadWithType?type=" + imageType;
                }
                else
                {
                    saveUrl = url + saveUrl;
                }

            }
            if (removeUrl == "/Common/RemoveFileUpload")
            {
                if (isFakeRemove)
                {
                    removeUrl = url + "/Common/RemoveFileFake";
                }
                else if (imageType != 0)
                {
                    removeUrl = url + "/Common/RemoveFileUploadWithType?type=" + imageType;
                }
                else
                {
                    removeUrl = url + removeUrl;
                }
            }
            var viewModel = new FileUploadViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                ClassCol = classcol,
                ClassLabel = classlabel,
                ClassField = classfield,
                Required = required,
                SaveUrl = saveUrl,
                RemoveUrl = removeUrl,
                PreviewHeight = previewHeight,
                PreviewWidth = previewWidth,
                AcceptType = acceptType,
                IsMultiple = isMultiple,
                IsUploadImage = isUploadImage,
                IsChangeAvatar = isChangeAvatar,
                IsAllowMultiFile = isAllowMultiFile,
                SelectText = selectText,
                IsReturnObject = isReturnObject,
                OnsuccessFunction = onsuccessFunction,
                IsUploadVideo = isUploadVideo
            };
            var partialTask = htmlHelper.PartialAsync("~/Views/Shared/Input/FileUploadAngular.cshtml", viewModel);
            return partialTask.Result;
        }
    }
}
