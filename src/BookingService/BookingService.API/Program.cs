using BookingService.API.ExceptionHandling;
using BookingService.Application.Extensions;
using BookingService.Infrastructure.Extensions;
using BookingService.Application.MassageBroker;
using BookingService.Infrastructure.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

services.Configure<EmailNotificationOptions>(configuration
    .GetSection(nameof(EmailNotificationOptions)));

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

app.UseHttpsRedirection();

app.Run();
