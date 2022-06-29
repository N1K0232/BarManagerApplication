using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    protected override void OnConfigure(EntityTypeBuilder<Category> builder)
    {
        base.OnConfigure(builder);
        builder.ToTable("Categories");
        builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(512).IsRequired(false);
    }
}