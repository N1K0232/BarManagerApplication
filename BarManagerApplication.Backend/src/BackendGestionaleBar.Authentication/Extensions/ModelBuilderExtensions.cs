using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackendGestionaleBar.Authentication.Extensions;

public static class ModelBuilderExtensions
{
    private static readonly ValueConverter<string, string> trimStringConverter;

    static ModelBuilderExtensions()
    {
        trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
    }

    public static ModelBuilder ApplyTrimStringConverter(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    builder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(trimStringConverter);
                }
            }
        }

        return builder;
    }
}