using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.DataAccess.Extensions
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
