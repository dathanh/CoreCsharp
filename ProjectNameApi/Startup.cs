using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Framework.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ProjectName.Services.Container;
using ProjectNameApi.Filter;
using System;

namespace ProjectNameApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var frontEndUrl = Configuration.GetSection("FrontEndUrl").Value;
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .WithOrigins(frontEndUrl)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
            services.AddMvc(opt =>
            {
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
               .AddJsonOptions(opt =>
               {
                   opt.SerializerSettings.Converters.Add(new ValidEnumConverter());
               });
            services.AddAutoMapper();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterModule<CoreModule>();
            containerBuilder.RegisterModule<WebModule>();

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            AppDependencyResolver.Init(app.ApplicationServices);
            app.UseCors("AllowAll");
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Index",
                    template: "san-pham/samsung-galaxy-note-10",
                    defaults: new { controller = "HomeMain", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=HomeMain}/{action=IndexHome}/{id?}");
            });
        }
    }
}
