using Autofac;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using ServiceLayer;
using ServiceLayer.BusinessRules;
using ServiceLayer.BusinessRules.Common;
using ServiceLayer.Common;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Common;
using ServiceLayer.Translate;
using ProjectName.Services.Deployment;
using ProjectName.Services.Email;
using ProjectName.Services.Youtube;

namespace ProjectName.Services.Container
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterWebService(builder);
            RegisterBusinessRules(builder);
        }

        private void RegisterWebService(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<WebDeploymentService>().As<IDeploymentService>().InstancePerLifetimeScope();
            //builder.RegisterType<SingletonJobFactory>().As<IJobFactory>().SingleInstance();
            //builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            //builder.RegisterType<GetViewNumberJobFromYoutube>().As<IJob>().SingleInstance();
            //builder.RegisterType<GetViewNumberJobFromYoutube>().As<IJob>().SingleInstance();
            //builder.RegisterType<GetViewNumberJobFromYoutube>().As<IJob>().SingleInstance();
            builder.RegisterType<FrontEndMessageLookup>().As<IFrontEndMessageLookup>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MasterFileService<>)).As(typeof(IMasterFileService<>));
            builder.RegisterType<ResizeImageService>().As<IResizeImage>();
            builder.RegisterType<TempUploadFileService>().As<ITempUploadFileService>();
            builder.RegisterType<YoutubeService>().As<IYoutubeService>().SingleInstance();
            builder.RegisterType<MenuExtractData>().As<IMenuExtractData>().SingleInstance();
            builder.RegisterType<ConfigSystem>().As<IConfigSystem>().SingleInstance();
            builder.RegisterType<EmailHandler>().As<IEmailHandler>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<GridConfigService>().As<IGridConfigService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<VideoService>().As<IVideoService>();
            builder.RegisterType<SeriesService>().As<ISeriesService>();
            builder.RegisterType<BannerService>().As<IBannerService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<ConfigService>().As<IConfigService>();
            builder.RegisterType<PlayListService>().As<IPlayListService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
        }


        private void RegisterBusinessRules(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DataAnnotationBusinessRule<>)).As(typeof(IBusinessRule<>));
            builder.RegisterType<BusinessRuleSet<User>>().AsImplementedInterfaces();
            builder.RegisterType<UserRule<User>>().AsImplementedInterfaces();
            builder.RegisterType<BusinessRuleSet<UserRole>>().AsImplementedInterfaces();
            builder.RegisterType<UserRoleRule<UserRole>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Category>>().AsImplementedInterfaces();
            builder.RegisterType<CategoryRule<Category>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Video>>().AsImplementedInterfaces();
            builder.RegisterType<VideoRule<Video>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Series>>().AsImplementedInterfaces();
            builder.RegisterType<SeriesRule<Series>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Banner>>().AsImplementedInterfaces();
            builder.RegisterType<BannerRule<Banner>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Config>>().AsImplementedInterfaces();
            builder.RegisterType<ConfigRule<Config>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<PlayList>>().AsImplementedInterfaces();
            builder.RegisterType<PlayListRule<PlayList>>().AsImplementedInterfaces();

        }
    }
}
