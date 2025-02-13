using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelService.DataAccess.Configurations
{
    public class BookedDateEntityConfiguration : IEntityTypeConfiguration<BookedDateEntity>
    {
        public void Configure(EntityTypeBuilder<BookedDateEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.RoomId)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();
        }
    }
}
