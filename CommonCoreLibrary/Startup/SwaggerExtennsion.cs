using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CommonCoreLibrary.Startup
{
    public static class SwaggerExtennsion
    {
        public static void AddSwagger(this IServiceCollection services, string name, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = name,
                    Version = version
                });
            });
        }

        public static void UseSwaggerWithUI(this IApplicationBuilder app, string name, string version)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{name} {version}");
            });
        }
    }
}
