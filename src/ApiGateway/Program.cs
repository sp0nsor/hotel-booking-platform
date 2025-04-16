using ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.LoadMergedOcelotConfiguration();

services.AddHttpContextAccessor();

services.AddOcelot(configuration);

services.AddRedis(configuration);
services.AddJwtAuthentication(configuration);

var app = builder.Build();

app.UseAuthentication();

await app.UseOcelot();

app.Run();
