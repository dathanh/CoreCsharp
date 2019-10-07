using Framework.Service.Translation;
using System.ComponentModel.DataAnnotations;

namespace Framework.DataAnnotations
{
    public class LocalizeMaxLengthAttribute : MaxLengthAttribute

    {
        public LocalizeMaxLengthAttribute(int length) : base(length)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(SystemMessageLookup.GetMessage("MaxLengthRequied"), name, Length);
        }
    }
}