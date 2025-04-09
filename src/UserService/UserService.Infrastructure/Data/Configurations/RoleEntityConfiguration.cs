using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Enums.Users;

namespace UserService.Infrastructure.Data.Configurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired();

            var roles = Enum
                .GetValues<Roles>()
                .Select(role => new RoleEntity()
                {
                    Id = (int)role,
                    Name = role.ToString()
                });

            builder.HasData(roles);
        }
    }
}
