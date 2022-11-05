using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");
        builder.HasKey(c => c.Id);

        builder.Property(r => r.Value).IsRequired().HasMaxLength(5);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(r => r.Post)
            .WithMany()
            .HasForeignKey(c => c.PostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}