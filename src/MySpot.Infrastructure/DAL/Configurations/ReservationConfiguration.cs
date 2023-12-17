using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Configurations
{
    internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.ParkingSpotId)
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.Capacity)
                .IsRequired(true)
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.Date)
                .HasConversion(x => x.Value, y => y);

            builder
                .HasDiscriminator<string>("Discriminator")
                .HasValue<CleaningReservation>(nameof(CleaningReservation))
                .HasValue<VehicleReservation>(nameof(VehicleReservation));
        }
    }
}
