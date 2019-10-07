using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common;
using Framework.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ProjectName.Services.Container;
using ProjectName.Services.Scheduler;
using System;
using System.Threading.Tasks;

namespace ProjectName
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_cookie";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        var param = context.Request.QueryString.ToString();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    };
                });


            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opt.SerializerSettings.Converters.Add(new ValidEnumConverter());
                });
            services.AddHttpContextAccessor();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<GetViewNumberJobFromYoutube>();
            var setupSchedule = Configuration.GetSection("YoutubeScheduler").Value;
            if (string.IsNullOrEmpty(setupSchedule))
            {
                setupSchedule = "0 31 * * * ?";
            }
            services.AddSingleton(new JobSchedule(
              jobType: typeof(GetViewNumberJobFromYoutube),
              cronExpression: setupSchedule)); // run every 5 seconds
            services.AddHostedService<QuartzHostedService>();
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
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });
            app.UseAuthentication();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=MainContainer}/{action=Index}/{id?}");
            });
        }
    }
}
