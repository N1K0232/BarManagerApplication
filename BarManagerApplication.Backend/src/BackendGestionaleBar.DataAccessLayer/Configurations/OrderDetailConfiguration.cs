using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal class OrderDetailConfiguration : BaseEntityConfiguration<OrderDetail>
{
    protected override void OnConfigure(EntityTypeBuilder<OrderDetail> builder)
    {
        base.OnConfigure(builder);
        builder.ToTable("OrderDetails");
        builder.HasKey(od => new { od.OrderId, od.ProductId });
        builder.HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .IsRequired();

        builder.HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId)
            .IsRequired();
    }
}