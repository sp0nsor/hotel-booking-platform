using UserService.API.Extensions;
using UserService.Application.Extensions;
using UserService.Application.Options;
using UserService.Infrastructure.Extensions;
using UserService.API.ExceptionHandling;
using UserService.API.Grpc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.UseElk();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(configuration);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddJwtAuthentication(configuration);

services.Configure<RefreshTokenOptions>(configuration
    .GetSection(nameof(RefreshTokenOptions)));

services.Configure<AccessTokenOptions>(configuration
    .GetSection(nameof(AccessTokenOptions)));

services.Configure<EmailOptions>(configuration
    .GetSection(nameof(EmailOptions)));

services.Configure<ConfirmCodeOptions>(configuration
    .GetSection(nameof(ConfirmCodeOptions)));

services.AddExceptionHandler<GlobalExceptionHandler>();

services.AddProblemDetails();

var app = builder.Build();

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

app.MapControllers();

app.MapGrpcService<UserGrpcService>();

app.UseSerilogRequestLogging();

app.Run();
