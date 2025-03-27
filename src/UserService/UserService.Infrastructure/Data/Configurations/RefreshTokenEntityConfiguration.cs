using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Configurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Expires)
                .IsRequired()
                .HasConversion(
                    t => t.ToUniversalTime(),
                    t => DateTime.SpecifyKind(t, DateTimeKind.Utc));

            builder.HasOne(t => t.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshTokenEntity>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
