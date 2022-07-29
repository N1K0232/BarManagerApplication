using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace BackendGestionaleBar.DataAccessLayer.Extensions;

public static class ModelBuilderExtensions
{
    private static readonly ValueConverter<string, string> trimStringConverter;
    private static readonly MethodInfo setQueryFilter;

    static ModelBuilderExtensions()
    {
        trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
        setQueryFilter = typeof(DataContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetQueryFilter");
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

    public static ModelBuilder ApplyQueryFilter(this ModelBuilder builder, DataContext dataContext)
    {
        var entities = builder.Model
            .GetEntityTypes()
            .Where(t => typeof(DeletableEntity).IsAssignableFrom(t.ClrType))
            .ToList();

        foreach (var type in entities.Select(t => t.ClrType))
        {
            var methods = SetGlobalQueryMethods(type);

            foreach (var method in methods)
            {
                var genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(dataContext, new object[] { builder });
            }
        }

        return builder;
    }

    private static IEnumerable<MethodInfo> SetGlobalQueryMethods(Type type)
    {
        var result = new List<MethodInfo>();

        if (typeof(DeletableEntity).IsAssignableFrom(type))
        {
            result.Add(setQueryFilter);
        }

        return result;
    }
}