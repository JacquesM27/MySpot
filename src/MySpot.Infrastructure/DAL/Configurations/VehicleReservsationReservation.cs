using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Configurations
{
    internal sealed class VehicleReservsationReservation : IEntityTypeConfiguration<VehicleReservation>
    {
        public void Configure(EntityTypeBuilder<VehicleReservation> builder)
        {
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.EmployeeName)
               .HasConversion(x => x.Value, y => y);

            builder.Property(x => x.LicensePlate)
                .HasConversion(x => x.Value, y => y);
        }
    }
}
