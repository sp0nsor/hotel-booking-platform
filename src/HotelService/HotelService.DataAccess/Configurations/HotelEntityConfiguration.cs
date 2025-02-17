using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelService.DataAccess.Configurations
{
    public class HotelEntityConfiguration : IEntityTypeConfiguration<HotelEntity>
    {
        public void Configure(EntityTypeBuilder<HotelEntity> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired();

            builder.Property(h => h.Description)
                .IsRequired();

            builder.Property(h => h.PhoneNumber)
                .IsRequired();

            builder.Property(h => h.Country)
                .IsRequired();

            builder.Property(h => h.City)
                .IsRequired();

            builder.Property(h => h.Street)
                .IsRequired();

            builder.Property(h => h.ImageUrl)
                .IsRequired();

            builder.Property(h => h.PriceCategory)
                .IsRequired();

            builder.HasMany(h => h.Rooms)
                .WithOne()
                .HasForeignKey(r  => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
