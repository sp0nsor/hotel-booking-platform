using HotelService.API.Hubs;
using HotelService.API.Extensions;
using HotelService.API.Grpc.Hotel;
using HotelService.API.Grpc.Room;
using HotelService.Application.Extensions;
using HotelService.DataAccess.Extensions;
using HotelService.Infrastructure.Extensions;
using HotelService.Infrastructure.MessageBroker;
using Serilog;
using HotelService.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.UseELK();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddJwtAuthentication(configuration);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.Configure<MessageBrokerOptions>(configuration
    .GetSection(nameof(MessageBrokerOptions)));

services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddDataAccess(configuration);

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

app.MapGrpcService<HotelGrpcService>();
app.MapGrpcService<RoomGrpcService>();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapHub<EntityHub<HotelDto>>("/hotelHub");
app.MapHub<EntityHub<RoomDto>>("/roomHub");

app.UseSerilogRequestLogging();

app.Run();
