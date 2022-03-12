using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations
{
    public class AllergenConfiguration : BaseEntityConfiguration<Allergen>
    {
        public override void Configure(EntityTypeBuilder<Allergen> builder)
        {
            base.Configure(builder);

            builder.ToTable("Allergens");
            builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(512);
        }
    }
}