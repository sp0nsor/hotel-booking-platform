using BookingService.API.ExceptionHandling;
using BookingService.Application.Extensions;
using BookingService.Infrastructure.Extensions;
using BookingService.API.Extensions;
using BookingService.Application.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.UseElk();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddJwtAuthentication(configuration);

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

services.Configure<EmailOptions>(configuration
    .GetSection(nameof(EmailOptions)));

services.Configure<MessageBrokerOptions>(configuration
    .GetSection(nameof(MessageBrokerOptions)));

services.AddExceptionHandler<GlobalExceptionHandler>();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.Run();
