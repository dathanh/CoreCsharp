using Autofac;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.Service.Diagnostics;
using Microsoft.AspNetCore.Http;
using ProjectName.Services.Deployment;
using ServiceLayer;
using ServiceLayer.BusinessRules.Common;
using ServiceLayer.Interfaces;

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
            builder.RegisterGeneric(typeof(MasterFileService<>)).As(typeof(IMasterFileService<>));
            builder.RegisterType<DiagnosticService>().As<IDiagnosticService>().InstancePerLifetimeScope();

            builder.RegisterType<EmailHandler>().As<IEmailHandler>().SingleInstance();

        }


        private void RegisterBusinessRules(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DataAnnotationBusinessRule<>)).As(typeof(IBusinessRule<>));

        }
    }
}