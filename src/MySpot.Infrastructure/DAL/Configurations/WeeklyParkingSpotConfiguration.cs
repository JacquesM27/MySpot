using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Configurations
{
    internal sealed class WeeklyParkingSpotConfiguration : IEntityTypeConfiguration<WeeklyParkingSpot>
    {
        public void Configure(EntityTypeBuilder<WeeklyParkingSpot> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.Week)
                .HasConversion(x => x.To.Value, y => new Core.ValueObjects.Week(y));

            builder.Property(x => x.Name)
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.Capacity)
                .IsRequired(true)
                .HasConversion(x => x.Value, y => y);
        }
    }
}
