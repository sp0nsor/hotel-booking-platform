using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelService.DataAccess.Configurations
{
    public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder.HasKey(r =>  r.Id);

            builder.Property(r => r.HotelId)
                .IsRequired();

            builder.Property(r => r.Capacity)
                .IsRequired();

            builder.Property(r => r.Area)
                .IsRequired();

            builder.Property(r => r.Number)
                .IsRequired();

            builder.Property(r => r.MoneyAmount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(r => r.Currency)
                .IsRequired();

            builder.Property(r => r.ImageUrl)
                .IsRequired();

            builder.HasMany(r => r.BookedDateEntities)
                .WithOne()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
