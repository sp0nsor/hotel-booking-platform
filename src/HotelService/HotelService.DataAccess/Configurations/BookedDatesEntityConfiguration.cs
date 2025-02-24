using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelService.DataAccess.Configurations
{
    public class BookedDatesEntityConfiguration : IEntityTypeConfiguration<BookedDatesEntity>
    {
        public void Configure(EntityTypeBuilder<BookedDatesEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.RoomId)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.HasIndex(b => b.RoomId);
        }
    }
}
