using BAMS.Data;
using BAMS.Data.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAMS.Helpers;
using BAMS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;
using Bams.Workflows;
using Bams.Workflows.Default;
using EightElements.Services.Default;
using EightElements.Services;
using EightElements.Services.Models;
using Bams.Workflows.InputValidators;

namespace BAMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);

            services.AddMemoryCache();
            services.AddControllers();
            services.AddHttpClient();
            services.AddMvcCore();
            services.AddControllersWithViews();
            
            services.Configure<MailSetting>(Configuration.GetSection("MailSetting"));
            
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie = new CookieBuilder
                    {
                        Name = "auth",
                        SameSite = SameSiteMode.Strict
                    };
                    //options.EventsType = typeof(AppCookieAuthenticationEvents);
                    options.LoginPath = "/login";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.SlidingExpiration = true;
                    //options.AccessDeniedPath = "/Account/AccessDenied";
                });
            services.AddSession(options =>
            {
                options.Cookie.Name = ".ShowTime.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });           
                        
            services.AddHttpContextAccessor();

            ConfigureDependencies(services);
        }

        public void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ITextService, TextService>();
            services.AddScoped<IChangelogService, ChangelogService>();
            services.AddScoped<IProjectWorkflow, ProjectWorkflow>();
            services.AddScoped<IProjectValidator, ProjectValidator>();
            services.AddScoped<IContractWorkflow, ContractWorkflow>();
            services.AddScoped<IContractValidator, ContractValidator>();
            services.AddScoped<ISchoolWorkflow, SchoolWorkflow>();
            services.AddScoped<ISchoolValidator, SchoolValidator>();
            services.AddScoped<IStudentWorkflow, StudentWorkflow>();
            services.AddScoped<IStudentValidator, StudentValidator>();
            services.AddScoped<IAdministrativeUnitValidator, AdministrativeUnitValidator>();
            services.AddScoped<IAdministrativeUnitWorkflow, AdministrativeUnitWorkflow>();
            services.AddScoped<ILanguageProvider, LanguageProvider>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
