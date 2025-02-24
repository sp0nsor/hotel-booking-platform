using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelService.DataAccess
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) 
            : base(options) { }

        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<BookedDatesEntity> BookedDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
        }
    }
}
