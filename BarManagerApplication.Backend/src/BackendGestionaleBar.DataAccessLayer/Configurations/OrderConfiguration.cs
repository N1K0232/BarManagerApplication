using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal sealed class OrderConfiguration : DeletableEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);
        builder.ToTable("Orders");

        builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(o => o.OrderDate).IsRequired();
        builder.Property(o => o.UserId).IsRequired();
        builder.HasOne(o => o.Umbrella).WithMany(u => u.Orders).HasForeignKey(o => o.UmbrellaId).IsRequired();
    }
}