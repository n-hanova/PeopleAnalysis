using AnalyticAPI.Services.Settings;
using AuthAPI.Settings;
using CommonCoreLibrary.Auth.Interfaces;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeopleAnalysis.ApplicationAPI;
using PeopleAnalysis.AuthAPI;
using PeopleAnalysis.Models;
using PeopleAnalysis.Models.Configuration;
using PeopleAnalysis.Services;
using System.Net.Http;

namespace PeopleAnalysis
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
            var authSettings = Configuration.GetSection("AuthSettings").Get<AuthSettings>();
            services.AddSingleton(authSettings);

            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Auth/Login";
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(Configuration.Get<KeysConfiguration>());
            services.AddSingleton<ColorService>();
            services.AddScoped<ILocalizer, Localizer>();

            services.AddSingleton<IBaseTokenService, ClientTokenService>();
            services.AddSingleton<IAuthAPIClient, AuthAPIClient>(x => new AuthAPIClient(Configuration["AuthAPI"], new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } }),
                x.GetRequiredService<IHttpContextAccessor>(), x.GetRequiredService<IBaseTokenService>()));
            services.AddSingleton<IApplicationAPIClient, ApplicationAPIClient>(x => new ApplicationAPIClient(Configuration["ApplicationAPI"], new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } }),
                x.GetRequiredService<IHttpContextAccessor>(), x.GetRequiredService<IBaseTokenService>(), x.GetRequiredService<IAuthAPIClient>()));
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
