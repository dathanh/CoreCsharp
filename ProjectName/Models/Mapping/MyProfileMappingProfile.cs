using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using ProjectName.Models.MyProfile;

namespace ProjectName.Models.MappingProfile
{
    public class MyProfileMappingProfile : Profile
    {
        public MyProfileMappingProfile()
        {
            CreateMap<Framework.DomainModel.Entities.User, DashboardMyProfileDataViewModel>()
                .AfterMap((s, d) =>
                {
                    d.SharedViewModel = s.MapTo<DashboardMyProfileShareViewModel>();
                });
            CreateMap<Framework.DomainModel.Entities.User, DashboardMyProfileShareViewModel>()
                .ForMember(o => o.Avatar, opt => opt.Ignore())
                .ForMember(o => o.Phone, opt => opt.Ignore()).AfterMap((s, d) =>
                {
                    d.Phone = s.Phone.ApplyFormatPhone();
                });

            CreateMap<DashboardMyProfileDataViewModel, Framework.DomainModel.Entities.User>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });

            CreateMap<DashboardMyProfileShareViewModel, Framework.DomainModel.Entities.User>()
                .ForMember(o => o.Avatar, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Phone = s.Phone.RemoveFormat();
                });

        }

    }
}