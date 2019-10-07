using Autofac;
using Autofac.Extras.DynamicProxy;
using Database.Persistance.Tenants;
using Framework.Exceptions.DataAccess.Interceptor;
using Framework.Exceptions.DataAccess.Meta;
using Framework.Exceptions.DataAccess.Translator;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Repositories;
using Repositories.Interfaces;
using ServiceLayer.Authentication;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces;

namespace ProjectName.Services.Container
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //base.Load(builder);

            RegisterServices(builder);
            RegisterRepositories(builder);
        }


        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<TenantPersistenceService>().As<ITenantPersistenceService>().InstancePerLifetimeScope();
            builder.RegisterType<DiagnosticService>().As<IDiagnosticService>().SingleInstance();
            builder.RegisterType<ClaimsManager>().As<IClaimsManager>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<WebProjectHttpContext>().As<IWebProjectHttpContext>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsManager>().As<IClaimsManager>().InstancePerLifetimeScope();
            builder.RegisterType<OperationAuthorization>().As<IOperationAuthorization>();
            builder.RegisterType<XmlDataHelper>().As<IXmlDataHelpper>().SingleInstance();
        }

        private void RegisterRepositoriesInterceptor(ContainerBuilder builder)
        {
            builder.RegisterType<DataAccessExceptionInterceptor>();
        }

        private void RegisterTenantSystemRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<SqlServerDbMetaInfo>().As<IDbMetaInfo>();
            builder.RegisterType<EntityFrameworkExceptionTranslator>().As<IExceptionTranslator>();
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            RegisterRepositoriesInterceptor(builder);
            RegisterTenantSystemRepositories(builder);
            RegisterEntityFrameworkRepositories(builder);
        }

        private void RegisterEntityFrameworkRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<EntityFrameworkUserRepository>().As<IUserRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkUserRoleRepository>().As<IUserRoleRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkUserRoleFunctionRepository>().As<IUserRoleFunctionRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkGridConfigRepository>().As<IGridConfigRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCategoryRepository>().As<ICategoryRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkVideoRepository>().As<IVideoRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkSeriesRepository>().As<ISeriesRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkBannerRepository>().As<IBannerRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCustomerRepository>().As<ICustomerRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkConfigRepository>().As<IConfigRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkPlayListRepository>().As<IPlayListRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCustomerRepository>().As<ICustomerRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
        }

    }
}
