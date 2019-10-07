using Framework.Utility;
using System;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerGridVo : ReadOnlyGridVo
    {
        public string UserName { get; set; }
        public DateTime? Dob { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsAccountFacebook { get; set; }
        public bool? IsAccountGoogle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr => CreatedDate.GetValueOrDefault().ToString(ConstantValue.DateFormatWithHour);
        public string Type
        {
            get
            {
                if (IsAccountFacebook.GetValueOrDefault())
                {
                    return "Via Facebook";
                }
                if (IsAccountGoogle.GetValueOrDefault())
                {
                    return "Via Google";
                }
                return "Via Email";
            }
        }
    }
}