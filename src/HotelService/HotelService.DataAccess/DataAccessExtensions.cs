using HotelService.Core.Abstractions;
using HotelService.Core.Models;
using HotelService.DataAccess.Entities;
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

            services.AddScoped<IRepository<Hotel>, Repository<Hotel, HotelEntity>>();
            services.AddScoped<IRepository<Room>, Repository<Room, RoomEntity>>();

            services.AddAutoMapper(typeof(HotelProfile));
            services.AddAutoMapper(typeof(RoomProfile));
            services.AddAutoMapper(typeof(BookedDatesEntity));

            return services;
        }
    }
}
