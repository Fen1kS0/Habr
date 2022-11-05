using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Text).IsRequired().HasMaxLength(2000);
        builder.Property(p => p.Rating).IsRequired().HasPrecision(2, 1);

        builder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(dp => dp.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder
            .HasOne(dp => dp.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(dp => dp.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}