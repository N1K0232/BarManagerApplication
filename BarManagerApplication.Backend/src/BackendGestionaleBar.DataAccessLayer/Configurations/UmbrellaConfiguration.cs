using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal sealed class UmbrellaConfiguration : DeletableEntityConfiguration<Umbrella>
{
    public override void Configure(EntityTypeBuilder<Umbrella> builder)
    {
        base.Configure(builder);

        builder.ToTable("Umbrellas");
        builder.Property(u => u.Coordinates)
            .HasMaxLength(10)
            .IsRequired();
    }
}