using AnalyticAPI.Services.Settings;
using CommonCoreLibrary.Services;
using CommonCoreLibrary.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeopleAnalisysAPI.Services;
using PeopleAnalysis.Models.Configuration;
using PeopleAnalysis.Services;
using PeopleAnalysis.Services.APIs;

namespace PeopleAnalisysAPI
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
            services.AddJwtAuth(Configuration);
            services.AddSwagger(Configuration["ApiInfo:Name"], Configuration["ApiInfo:Version"]);
            services.AddDbContext<IDatabaseContext, DatabaseContext>(x => x.UseNpgsql(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies(), ServiceLifetime.Scoped);

            services.AddScoped<ApisManager>();
            services.AddSingleton(Configuration.Get<KeysConfiguration>());
            services.AddScoped<AnaliticService>();
            services.AddScoped<IAnaliticAIService, AnaliticAIService>();
            services.AddScoped<VkSocialApi>();
            services.AddScoped<ISender, RabbitMQClient>();
            services.AddControllers();

            services.AddSingleton<IMapperService, MapperService>();
            services.AddHttpContextAccessor();

            services.AddSingleton(Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseContext databaseContext)
        {
            databaseContext.ApplyMigration();

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
