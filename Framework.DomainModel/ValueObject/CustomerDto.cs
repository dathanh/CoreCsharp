using System;
using Framework.DomainModel.Entities.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerDto : DtoBase
    {
        private readonly IXmlDataHelpper _xmlDataHelpper;

        public CustomerDto()
        {
            _xmlDataHelpper = AppDependencyResolver.Current.GetService<IXmlDataHelpper>();
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int? Gender { get; set; }
        public string Description { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        public bool? IsAccountFacebook { get; set; }
        public bool? IsAccountGoogle { get; set; }
        public int? LanguageId { get; set; }
        public string CategoryConfig { get; set; }

        public string GenderFormat => _xmlDataHelpper.GetValue(XmlDataTypeEnum.GenderType.ToString(), Gender.GetValueOrDefault().ToString());
    }
}