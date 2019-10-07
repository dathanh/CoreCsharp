using Framework.Service.Translation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.DataAnnotations
{
    /// <summary>
    /// Override the required attribute in case validate property is required with message
    /// </summary>
    public class LocalizeRequiredAttribute : RequiredAttribute
    {
        public string FieldName { get; set; }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), String.IsNullOrWhiteSpace(FieldName) ? name : FieldName);
        }
    }
}
