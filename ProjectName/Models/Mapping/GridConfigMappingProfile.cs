using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.Utility;
using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.Models.Mapping
{
    public class GridConfigMappingProfile : Profile
    {
        public GridConfigMappingProfile()
        {
            CreateMap<GridConfig, GridConfigViewModel>()
                .AfterMap(
                    (s, d) =>
                    {
                        d.ViewColumns = SerializationHelper.Deserialize<List<ViewColumnViewModel>>(s.XmlText);
                        if (d.ViewColumns != null)
                        {
                            d.ViewColumns.ForEach(c => c.Text = c.Text ?? string.Empty);
                        }
                        //enable by default, later may come back to get from config
                        d.AllowReorderColumn = true;
                        d.AllowResizeColumn = true;
                        d.AllowShowHideColumn = true;
                    });

            CreateMap<GridConfigViewModel, GridConfig>()
                .ForMember(x => x.XmlText, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .AfterMap((s, d) => d.XmlText = SerializationHelper.SerializeToXml(s.ViewColumns));
        }
    }
}