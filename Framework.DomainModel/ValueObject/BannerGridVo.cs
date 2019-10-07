using Framework.DomainModel.Entities.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class BannerGridVo : ReadOnlyGridVo
    {
        private readonly IXmlDataHelpper _xmlDataHelper;
        public BannerGridVo()
        {
            _xmlDataHelper = AppDependencyResolver.Current.GetService<IXmlDataHelpper>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public string ParentName { get; set; }
        public string UrlLink { get; set; }
        public int? OrderNumber { get; set; }
        public int? Type { get; set; }
        public bool? IsHideDescription { get; set; }
        public string TimeDuration { get; set; }

        public string TypeFormat => _xmlDataHelper.GetValue(XmlDataTypeEnum.BannerType.ToString(), Type.GetValueOrDefault().ToString());
    }
}