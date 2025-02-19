using HotelService.Core.Abstractions;
using HotelService.DataAccess.Mappings;
using HotelService.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<HotelDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(HotelDbContext)));
            });

            services.AddScoped<IHotelRepository, HotelRepository>();

            services.AddAutoMapper(typeof(HotelProfile));
            services.AddAutoMapper(typeof(RoomProfile));

            return services;
        }
    }
}
