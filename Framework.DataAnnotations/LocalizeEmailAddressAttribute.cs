using Framework.Service.Translation;
using System.ComponentModel.DataAnnotations;

namespace Framework.DataAnnotations
{

    public class LocalizeEmailAddressAttribute : RegularExpressionAttribute
    {
        public LocalizeEmailAddressAttribute()
            : base(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$")
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(SystemMessageLookup.GetMessage("EmailValid"), name);
        }
    }
}