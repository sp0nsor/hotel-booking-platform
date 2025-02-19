using HotelService.Application;
using HotelService.DataAccess;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMediatR(x =>
    x.RegisterServicesFromAssemblies(Assembly.Load("HotelService.Application")));

services
    .AddApplication()
    .AddDataAccess(configuration);

services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
