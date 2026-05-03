using Application.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.WebApp.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskManagerAppService, TaskManagerAppService>();
            
            services.AddDbContext<TaskContext>(options => 
                options.UseSqlite("Filename=:memory:"));

            return services;
        }
    }
}
