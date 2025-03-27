using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique(true);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .IsRequired();

            builder.Property(u => u.LastName)
                .IsRequired();

            builder.Property(u => u.PhoneNumber)
                .IsRequired();

            builder.Property(u => u.IsActivated)
                .HasDefaultValue(false);

            builder.HasOne(u => u.RefreshToken)
                .WithOne(t => t.User)
                .HasForeignKey<UserEntity>(u => u.RefreshTokenId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
