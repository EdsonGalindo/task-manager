using Application.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace TaskManager.WebApp.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskManagerAppService, TaskManagerAppService>();

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            services.AddSingleton(connection);

            services.AddDbContext<TaskContext>((serviceProvider, options) =>
            {
                var sqliteConnection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(sqliteConnection);
            });

            return services;
        }
    }
}
