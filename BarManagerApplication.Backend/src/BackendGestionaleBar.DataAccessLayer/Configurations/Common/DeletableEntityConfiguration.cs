using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations.Common;

internal abstract class DeletableEntityConfiguration<T> : BaseEntityConfiguration<T> where T : DeletableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.DeletedDate).IsRequired(false);
    }
}