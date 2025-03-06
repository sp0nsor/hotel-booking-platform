using BookingService.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Data.Configurations
{
    public class BookingEntityConfiguration
        : IEntityTypeConfiguration<BookingEntity>
    {
        public void Configure(EntityTypeBuilder<BookingEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.HotelId)
                .IsRequired();

            builder.Property(b => b.RoomId)
                .IsRequired();

            builder.Property(b => b.GuestFirstName)
                .IsRequired();

            builder.Property(b => b.GuestLastName)
                .IsRequired();

            builder.Property(b => b.GuestEmail)
                .IsRequired();

            builder.Property(b => b.GuestPhoneNumber)
                .IsRequired();

            builder.Property(b => b.IsOutdated)
                .IsRequired()
                .HasDefaultValue(false);

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

            builder.Property(b => b.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.HasIndex(b => b.HotelId);
        }
    }
}
