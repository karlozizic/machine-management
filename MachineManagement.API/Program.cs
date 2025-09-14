using System.Data;
using System.Reflection;
using FluentMigrator.Runner;
using MachineManagement.API.Repositories;
using MachineManagement.API.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Fluent Migrator
builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

//register services within DI container
builder.Services.AddScoped<IDbConnection>(serviceProvider => new NpgsqlConnection(connectionString));
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMalfunctionRepository, MalfunctionRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IMalfunctionService, MalfunctionService>();

var app = builder.Build();

//apply migrations at application startup
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();