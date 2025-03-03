using HotelService.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HotelService.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using HotelDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<HotelDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
