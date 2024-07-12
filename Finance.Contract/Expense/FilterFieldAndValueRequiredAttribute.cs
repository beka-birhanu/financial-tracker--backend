using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Expense;

public class FilterFieldAndValueRequiredAttribute : ValidationAttribute
{
  private static readonly string[] AllowedFields = { "Amount", "Date", "Title" };

  protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
  {
    if (validationContext.ObjectInstance is not GetExpenseListQueryParams queryParams)
    {
      throw new ArgumentException("Attribute not applied on GetExpenseListQueryParams");
    }

    if (!string.IsNullOrEmpty(queryParams.filterField) && string.IsNullOrEmpty(queryParams.filterValue))
    {
      return new ValidationResult("FilterValue is required when filterField is provided.");
    }

    if (string.IsNullOrEmpty(queryParams.filterField) && !string.IsNullOrEmpty(queryParams.filterValue))
    {
      return new ValidationResult("filterField is required when FilterValue is provided.");
    }

    if (!string.IsNullOrEmpty(queryParams.filterField) && !AllowedFields.Contains(queryParams.filterField))
    {
      return new ValidationResult($"filterField must be one of [{string.Join(", ", AllowedFields)}].");
    }

    return ValidationResult.Success!;
  }
}

