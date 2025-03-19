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

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.HotelId)
                .IsRequired();

            builder.Property(b => b.HotelName)
                .IsRequired();

            builder.Property(b => b.Country)
                .IsRequired();

            builder.Property(b => b.City)
                .IsRequired();

            builder.Property(b => b.Street)
                .IsRequired();

            builder.Property(b => b.RoomId)
                .IsRequired();

            builder.Property(b => b.RoomNumber)
                .IsRequired();

            builder.Property(b => b.TotalPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(b => b.Currency)
                .IsRequired();

            builder.Property(b => b.IsOutdated)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(b => b.GuestFirstName)
                .IsRequired();

            builder.Property(b => b.GuestLastName)
                .IsRequired();

            builder.Property(b => b.GuestPhoneNumber)
                .IsRequired();

            builder.Property(b => b.GuestEmail)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired()
                .HasConversion(
                    p => p.ToUniversalTime(),
                    p => DateTime.SpecifyKind(p, DateTimeKind.Utc));

            builder.Property(b => b.EndDate)
                .IsRequired()
                .HasConversion(
                    p => p.ToUniversalTime(),
                    p => DateTime.SpecifyKind(p, DateTimeKind.Utc));

            builder.HasIndex(b => b.HotelId);
            builder.HasIndex(b => b.UserId);
        }
    }
}
