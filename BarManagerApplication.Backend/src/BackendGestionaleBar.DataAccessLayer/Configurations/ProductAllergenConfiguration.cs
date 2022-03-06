using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations
{
    public class ProductAllergenConfiguration : IEntityTypeConfiguration<ProductAllergen>
    {
        public void Configure(EntityTypeBuilder<ProductAllergen> builder)
        {
            builder.ToTable("ProductAllergens");

            builder.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAllergens)
                .HasForeignKey(pa => pa.IdProduct)
                .IsRequired();

            builder.HasOne(pa => pa.Allergen)
                .WithMany(a => a.ProductAllergens)
                .HasForeignKey(pa => pa.IdAllergen)
                .IsRequired();
        }
    }
}