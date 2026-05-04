using Data;
using TaskManager.WebApp.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();
builder.Services.RegisterDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

#region Garante que o banco foi criado ao iniciar a aplicação, utilizando o contexto de banco de dados para criar o banco caso ele ainda não exista.
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<TaskContext>();
context.Database.EnsureCreated();
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
