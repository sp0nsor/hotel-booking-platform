using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Interfaces.Data;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Repositories;
using BookingService.Infrastructure.Services.EmailService;
using BookingService.Infrastructure.Interfaces.Services;
using MongoDB.Driver;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire;
using BookingService.Infrastructure.Services.Grpc.Hotel;
using BookingService.Infrastructure.Services.Grpc.Room;

namespace BookingService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRepository<BookingEntity>, Repository<BookingEntity>>();

            services.AddScoped<IHotelGrpcClient, HotelGrpcClient>();
            services.AddScoped<IRoomGrpcClient, RoomGrpcClient>();

            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(BookingDbContext)));
            });

            var mongoUrl = MongoUrl.Create(configuration.GetConnectionString("HangfireDb"));
            var mongoClient = new MongoClient(mongoUrl);

            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                },
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            };

            services.AddHangfire(options =>
            {
                options.UseMongoStorage(mongoClient, mongoUrl.DatabaseName, storageOptions);
            });

            services
                .AddGrpcClient<HotelService.HotelServiceClient>(options =>
                {
                    options.Address = new Uri("https://hotel-service:8081");
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    return handler;
                });

            services.AddGrpcClientWithCustomHandler<RoomService.RoomServiceClient>("https://hotel-service:8081");
            services.AddGrpcClientWithCustomHandler<HotelService.HotelServiceClient>("https://hotel-service:8081");

            services.AddHangfireServer();

            return services;
        }

        private static IServiceCollection AddGrpcClientWithCustomHandler<TClient>(
            this IServiceCollection services, string serviceUrl)
            where TClient : class
        {
            services.AddGrpcClient<TClient>(options =>
            {
                options.Address = new Uri(serviceUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return handler;
            });

            return services;
        }
    }
}
