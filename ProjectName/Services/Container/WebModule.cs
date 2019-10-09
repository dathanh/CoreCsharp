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
            builder.RegisterType<ConfigService>().As<IConfigService>();
            //=====Import Service=======//
        }


        private void RegisterBusinessRules(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DataAnnotationBusinessRule<>)).As(typeof(IBusinessRule<>));
            builder.RegisterType<BusinessRuleSet<User>>().AsImplementedInterfaces();
            builder.RegisterType<UserRule<User>>().AsImplementedInterfaces();
            builder.RegisterType<BusinessRuleSet<UserRole>>().AsImplementedInterfaces();
            builder.RegisterType<UserRoleRule<UserRole>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Config>>().AsImplementedInterfaces();
            builder.RegisterType<ConfigRule<Config>>().AsImplementedInterfaces();
            //=====Import RegisterBusinessRules=======//

        }
    }
}
