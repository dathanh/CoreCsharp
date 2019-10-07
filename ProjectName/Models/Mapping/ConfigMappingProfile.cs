using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.Config;
using System.IO;
using System.Linq;

namespace ProjectName.Models.Mapping
{
    public class ConfigMappingProfile : Profile
    {
        public ConfigMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.Config, DashboardConfigDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardConfigShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.Config, DashboardConfigShareViewModel>().AfterMap((s, d) =>
            {
            });

            CreateMap<DashboardConfigDataViewModel, Framework.DomainModel.Entities.Config>().AfterMap((s, d) =>
            {
                s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardConfigShareViewModel, Framework.DomainModel.Entities.Config>().AfterMap((s, d) =>
            {
            });

        }
    }
}