using AutoMapper;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.User;

namespace ProjectName.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.User, UserDto>().ReverseMap().AfterMap((s, d) =>
            {
                s.UserRoleName = d.UserRole != null ? d.UserRole.Name : "";
            });
            CreateMap<UserDto, Framework.DomainModel.Entities.User>().ReverseMap();
            CreateMap<Framework.DomainModel.Entities.User, DashboardUserDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardUserShareViewModel>();
            });
            CreateMap<Framework.DomainModel.Entities.User, DashboardUserShareViewModel>()
                .ForMember(d => d.Phone, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Phone = s.Phone.ApplyFormatPhone();
                });

            CreateMap<DashboardUserDataViewModel, Framework.DomainModel.Entities.User>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            CreateMap<DashboardUserShareViewModel, Framework.DomainModel.Entities.User>()
                 .ForMember(d => d.Phone, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Phone = s.Phone.RemoveFormatPhone();
                });
        }

    }
}