using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations;

internal class ImageConfiguration : BaseEntityConfiguration<Image>
{
    public override void Configure(EntityTypeBuilder<Image> builder)
    {
        base.Configure(builder);
        builder.ToTable("Images");
        builder.Property(i => i.Path).HasMaxLength(256).IsRequired();
        builder.Property(i => i.Length).IsRequired();
    }
}