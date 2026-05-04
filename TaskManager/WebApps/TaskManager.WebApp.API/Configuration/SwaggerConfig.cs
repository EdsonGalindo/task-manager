using Microsoft.OpenApi;

namespace TaskManager.WebApp.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API - Sistema de Gerenciamento de Tarefas",
                    Version = "v1"
                });

                c.EnableAnnotations();
            });

            return services;
        }
    }
}
