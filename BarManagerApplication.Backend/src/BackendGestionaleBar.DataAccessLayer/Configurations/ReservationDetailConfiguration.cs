using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations
{
    internal class ReservationDetailConfiguration : BaseEntityConfiguration<ReservationDetail>
    {
        public override void Configure(EntityTypeBuilder<ReservationDetail> builder)
        {
            builder.HasKey(rd => new { rd.IdReservation, rd.IdProduct });

            builder.HasOne(rd => rd.Reservation)
                .WithMany(r => r.ReservationDetails)
                .HasForeignKey(rd => rd.IdReservation)
                .IsRequired();

            builder.HasOne(rd => rd.Product)
                .WithMany(p => p.ReservationDetails)
                .HasForeignKey(rd => rd.IdProduct)
                .IsRequired();
        }
    }
}