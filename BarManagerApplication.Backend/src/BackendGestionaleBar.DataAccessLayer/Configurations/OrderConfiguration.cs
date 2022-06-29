using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal class OrderConfiguration : BaseEntityConfiguration<Order>
{
    protected override void OnConfigure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);
        builder.ToTable("Orders");
        builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(o => o.OrderDate).IsRequired();
    }
}