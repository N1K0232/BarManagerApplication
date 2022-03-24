using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations
{
    internal class ReservationConfiguration : BaseEntityConfiguration<Reservation>
    {
        public override void Configure(EntityTypeBuilder<Reservation> builder)
        {
            base.Configure(builder);

            builder.ToTable("Reservations");

            builder.Property(r => r.Date).IsRequired();
            builder.Property(r => r.Time).IsRequired();

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.IdUser)
                .IsRequired();
        }
    }
}