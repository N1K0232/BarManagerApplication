using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal class ImageConfiguration : BaseEntityConfiguration<Image>
{
    protected override void OnConfigure(EntityTypeBuilder<Image> builder)
    {
        base.OnConfigure(builder);
        builder.ToTable("Images");
        builder.Property(i => i.Path).HasMaxLength(256).IsRequired();
        builder.Property(i => i.Length).IsRequired();
    }
}