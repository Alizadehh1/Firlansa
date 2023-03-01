using Firlansa.WebUI.AppCode.Providers;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;

namespace Firlansa.WebUI
{
    public class Startup
    {
        readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());

                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddRouting(cfg =>
            {
                cfg.LowercaseUrls = true;
            });

            services.AddDbContext<FirlansaDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("cString"));
            });

            services.AddIdentity<FirlansaUser, FirlansaRole>()
                .AddEntityFrameworkStores<FirlansaDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                //cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequiredLength = 3;

                cfg.User.RequireUniqueEmail = true;

                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 3, 0);
            });

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.ExpireTimeSpan = new TimeSpan(60, 0, 5, 0);
                cfg.Cookie.Name = "Firlansa";
            });

            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {

                foreach (var policyName in Program.principals)
                {
                    cfg.AddPolicy(policyName, p =>
                    {
                        p.RequireAssertion(handler =>
                        {
                            return handler.User.IsInRole("SuperAdmin")
                            || handler.User.HasClaim(policyName, "1");
                        });
                    });
                }

            });

            services.AddMediatR(this.GetType().Assembly);
            services.AddScoped<UserManager<FirlansaUser>>();
            services.AddScoped<SignInManager<FirlansaUser>>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IClaimsTransformation, AppClaimProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedUICultures("az", "en","ru");
                cfg.AddSupportedCultures("az", "en", "ru");
                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute(
                    name: "default",
                    pattern: "{lang}/{controller=home}/{action=index}/{id?}",
                    constraints: new
                    {
                        lang = "en|az|ru"
                    }
                    );

                cfg.MapAreaControllerRoute(
                    name: "defaultAdmin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
                cfg.MapControllerRoute("default", pattern: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
