using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelService.DataAccess.Configurations
{
    public class BookingPeriodEntityConfiguration : IEntityTypeConfiguration<BookingPeriodEntity>
    {
        public void Configure(EntityTypeBuilder<BookingPeriodEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.RoomId)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired()
                .HasConversion(
                    p => p.ToUniversalTime(),
                    p => DateTime.SpecifyKind(
                        p,
                        DateTimeKind.Utc));

            builder.Property(b => b.EndDate)
                .IsRequired()
                .HasConversion(
                    p => p.ToUniversalTime(),
                    p => DateTime.SpecifyKind(
                        p,
                        DateTimeKind.Utc));

            builder.HasIndex(b => b.RoomId);
        }
    }
}
