using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BackendGestionaleBar.Abstractions.Filters;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly IEnumerable<string> extensions;

    public AllowedExtensionsAttribute(params string[] extensions)
    {
        this.extensions = extensions.Select(e => e.ToLowerInvariant().Replace("*.", string.Empty));
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant()[1..];
            bool contains = extensions.Contains(extension);

            if (!contains)
            {
                string errorMessage = GetErrorMessage();
                return new ValidationResult(errorMessage);
            }
        }

        return ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        string mediaTypes = string.Join(",", extensions);
        return $"The supported media types are: {mediaTypes}";
    }
}