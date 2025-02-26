using HotelService.API;
using HotelService.Application;
using HotelService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddApi()
    .AddApplication()
    .AddDataAccess(configuration);

services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
