using UserService.API.Extensions;
using UserService.Application.Extensions;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Extensions;
using UserService.API.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services
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

app.ApplyMigrations();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
