using AnalyticAPI.ApplicationAPI;
using AnalyticAPI.AuthAPI;
using AnalyticAPI.Services;
using AnalyticAPI.Services.Settings;
using AutoMapper;
using CommonCoreLibrary.Auth.Interfaces;
using CommonCoreLibrary.Services;
using CommonCoreLibrary.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeopleAnalysis.Services;
using PeopleAnalysisML.Model;
using System.Net.Http;

namespace AnalyticAPI
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
            services.AddControllers();
            services.AddJwtAuth(Configuration);
            services.AddSwagger(Configuration["ApiInfo:Name"], Configuration["ApiInfo:Version"]);
            services.AddSingleton<IHttpContextAccessor, VirtualHttpContextAccessor>();

            services.AddHostedService<RabbitMQService>();
            services.AddSingleton<IAIService, TensorService>();
            services.AddSingleton<IMLService, ConsumeModel>(x => new ConsumeModel(Configuration["ML:ModelPath"]));
            services.AddSingleton<IMapperService, MapperService>();

            services.AddSingleton(Configuration.GetSection("Service").Get<ServiceAuthConfig>());
            services.AddSingleton(Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>());

            services.AddSingleton<IBaseTokenService, VirtualTokenService>();
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
