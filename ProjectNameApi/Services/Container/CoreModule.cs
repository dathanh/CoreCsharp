using Autofac;
using Autofac.Extras.DynamicProxy;
using Database.Persistance.Tenants;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions.DataAccess.Interceptor;
using Framework.Exceptions.DataAccess.Meta;
using Framework.Exceptions.DataAccess.Translator;
using Framework.Service.Diagnostics;
using Repositories;
using Repositories.Interfaces;

namespace ProjectName.Services.Container
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterRepositories(builder);
        }


        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<TenantPersistenceService>().As<ITenantPersistenceService>().InstancePerLifetimeScope();
            builder.RegisterType<DiagnosticService>().As<IDiagnosticService>().SingleInstance();
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
            //=====Import Repository=======//
        }

    }
}
