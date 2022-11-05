using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired();
        builder.Property(u => u.Email)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.RefreshToken).IsRequired(false);
        builder.Property(u => u.RefreshTokenExpiryTime).IsRequired(false);
        builder.Property(u => u.Role).IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();
    }
}