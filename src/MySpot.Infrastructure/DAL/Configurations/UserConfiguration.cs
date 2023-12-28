using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, y => y);

            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.Property(x => x.Email)
                .HasConversion(x => x.Value, y => y)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(x => x.Username)
                .IsUnique();
            builder.Property(x => x.Username)
                .HasConversion(x => x.Value, y => y)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Password)
                .HasConversion(x => x.Value, y => y)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Fullname)
                .HasConversion(x => x.Value, y => y)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Role)
                .HasConversion(x => x.Value, y => y)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
