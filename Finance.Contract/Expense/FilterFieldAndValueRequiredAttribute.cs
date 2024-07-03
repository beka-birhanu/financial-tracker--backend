using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Expense;

public class FilterFieldAndValueRequiredAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    var queryParams = (GetExpenseListQueryParams)validationContext.ObjectInstance;

    if (!string.IsNullOrEmpty(queryParams.filterField) && string.IsNullOrEmpty(queryParams.filterValue))
    {
      return new ValidationResult("FilterValue is required when filterField is provided.");
    }

    if (string.IsNullOrEmpty(queryParams.filterField) && !string.IsNullOrEmpty(queryParams.filterValue))
    {
      return new ValidationResult("filterField is required when FilterValue is provided.");
    }

    var allowedFields = new[] { "Amount", "Date", "Title" };
    if (!string.IsNullOrEmpty(queryParams.filterField) && !allowedFields.Contains(queryParams.filterField))
    {
      return new ValidationResult("filterField must be one of ['Amount', 'Date', 'Title'].");
    }

    return ValidationResult.Success;
  }
}

