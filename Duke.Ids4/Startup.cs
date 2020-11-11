using Duke.Ids4.Data;
using Duke.Ids4.Models;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Duke.Ids4
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(option => option.UseMySql(connectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //配置Authtication中间件 ,基于数据库配置
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction = new UserInteractionOptions
                {
                    //LoginUrl = "/oauth2/authorize",//登录地址
                };
            }).AddConfigurationStore(options =>
              {
                  options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
              })
              .AddOperationalStore(options =>
              {
                  options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                  options.EnableTokenCleanup = true;
              })
              .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                builder.AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                Configuration["Certificates:CerPath"]),
                Configuration["Certificates:Password"]));
            }

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:5004")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.InitializeDatabase();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("default");
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}