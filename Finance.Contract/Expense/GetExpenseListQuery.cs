using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Expense;

[FilterFieldAndValueRequired]
public record GetExpenseListQueryParams
{
  [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0")]
  public int pageNumber { get; init; } = 1;

  [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
  public int pageSize { get; init; } = 10;

  public string? filterField { get; init; }

  public string? filterValue { get; init; }

  [RegularExpression("Amount|Date", ErrorMessage = "SortField must be 'Amount' or 'Date'")]
  public string sortField { get; init; } = "Date";

  [RegularExpression("asc|desc", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
  public string sortOrder { get; init; } = "asc";
}

