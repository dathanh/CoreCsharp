using AutoMapper;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using ProjectName.Models.Role;

namespace ProjectName.Models.Mapping
{
    public class UserRoleMappingProfile : Profile
    {
        public UserRoleMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.UserRole, UserRoleDto>().ReverseMap().AfterMap((s, d) =>
            {
            });
            CreateMap<UserRoleDto, Framework.DomainModel.Entities.UserRole>().ReverseMap();
            CreateMap<Framework.DomainModel.Entities.UserRole, DashboardRoleDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardRoleShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.UserRole, DashboardRoleShareViewModel>().AfterMap((s, d) =>
            {
                if (s.Id != 0)
                {
                    d.LookupRole = new LookupItemVo()
                    {
                        DisplayName = s.Name,
                        KeyId = s.Id
                    };
                }

            });

            CreateMap<DashboardRoleDataViewModel, Framework.DomainModel.Entities.UserRole>()
                .AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardRoleShareViewModel, Framework.DomainModel.Entities.UserRole>()
                 .ForMember(o => o.AppRoleName, opt => opt.Ignore());
        }

    }
}