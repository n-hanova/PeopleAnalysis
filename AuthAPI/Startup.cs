using AuthAPI.Services;
using AuthAPI.Settings;
using CommonCoreLibrary.Services;
using CommonCoreLibrary.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthAPI
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
            services.AddDbContext<IAuthDataProvider, AuthDataProvider>(x => x.UseNpgsql(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies(), ServiceLifetime.Scoped);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IMapperService, AutoMapperService>();
            services.AddTransient<IUserService, UserService>();

            services.AddJwtAuth(Configuration);
            services.AddSwagger(Configuration["ApiInfo:Name"], Configuration["ApiInfo:Version"]);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAuthDataProvider authDataProvider, ILogger<Startup> logger)
        {
            logger.LogInformation($"Connection string: {Configuration.GetConnectionString("SQLServer")}");

            authDataProvider.ApplyChanges();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerWithUI(Configuration["ApiInfo:Name"], Configuration["ApiInfo:Version"]);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
