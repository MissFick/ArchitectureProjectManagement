using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Microsoft.EntityFrameworkCore;
using ArchitectureProjectManagement.Data;
using ArchitectureProjectManagement.Contracts;
using ArchitectureProjectManagement.Repository;
using ArchitectureProjectManagement.DataMapping;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;

namespace ArchitectureProjectManagement
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration, 
            IWebHostEnvironment env
            )
        {
            _configuration = configuration;
            _environment = env;
            
            _sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly bool _sslIsAvailable;
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(_configuration, _environment);
            
            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });

            //// **** IMPORTANT *****
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupDataStorage(_configuration, _environment);
            
            
            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                _configuration,
                _environment,
                _sslIsAvailable,
                _disableIdentityServer,
                out _didSetupIdServer
                );

            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(_configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization(_configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.ConsentCookie.Name = "cookieconsent_status";
            });

            services.Configure<Microsoft.AspNetCore.Mvc.CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(_sslIsAvailable);

            if(!_environment.IsDevelopment())
            {
                var httpsPort = _configuration.GetValue<int>("AppSettings:HttpsPort");
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = httpsPort;
                });
            }

            
            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseMySql(_configuration.GetConnectionString("EntityFrameworkConnection")));
                       
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProjectItemRepository, ProjectItemRepository>();
            services.AddScoped<IProjectItemStatusRepository, ProjectItemStatusRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IAppUserRoleRepository, AppUserRoleRepository>();
            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DataMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddScoped<UserStore<SiteUser>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
                if(_sslIsAvailable)
                {
                    app.UseHsts();
                }
            }
            if(_sslIsAvailable)
            {
                app.UseHttpsRedirection();
            }
            
            app.UseStaticFiles();
            app.UseCloudscribeCommonStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);
            app.UseCors("default"); //use Cors with policy named default, defined above

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore();

            if (!_disableIdentityServer && _didSetupIdServer)
            {
                app.UseIdentityServer();  
            }

#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
// workaround for 
//https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/466
 app.UseMvc(routes =>
            {
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingAndMvc.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders);
            });
#pragma warning restore MVC1005 // Cannot use UseMvc with Endpoint Routing.

//             app.UseEndpoints(endpoints =>
//             {
//                 var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
//                 //*** IMPORTANT ***
//                 // this is in Config/RoutingAndMvc.cs
//                 // you can change or add routes there
//                 endpoints.UseCustomRoutes(useFolders);
//             });
   
        }

        
        
    }
}