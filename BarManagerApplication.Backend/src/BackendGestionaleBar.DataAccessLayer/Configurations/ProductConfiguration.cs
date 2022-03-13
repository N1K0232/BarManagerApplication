﻿using BackendGestionaleBar.DataAccessLayer.Configurations.Common;
using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.DataAccessLayer.Configurations
{
    internal sealed class ProductConfiguration : BaseEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.ToTable("Products");
            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.IdCategory)
                .IsRequired();
        }
    }
}