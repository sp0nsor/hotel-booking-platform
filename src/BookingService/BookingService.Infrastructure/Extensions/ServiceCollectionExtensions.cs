using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Interfaces.Data;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Repositories;
using MongoDB.Driver;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire;

namespace BookingService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IRepository<BookingEntity>, Repository<BookingEntity>>();

            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(BookingDbContext)));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "local";
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

            services.AddHangfireServer();

            return services;
        }
    }
}
