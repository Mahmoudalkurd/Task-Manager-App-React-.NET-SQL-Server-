using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using backend.Services.Interfaces;
using backend.Services;
using backend.Infrastructure.Repositories;
using backend.API.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console() // logs to terminal
    .WriteTo.File(
        path: "Logs/log-.txt",   // Logs folder + daily file
        rollingInterval: RollingInterval.Day, // create new file each day
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

// Use Serilog for the host
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();
app.UseCors("AllowReactApp");

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
