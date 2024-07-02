using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Expense;

public record PaginationParameters
{
  [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0")]
  public int pageNumber { get; init; } = 1;

  [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
  public int pageSize { get; init; } = 10;
}

public record FilterParameters
{
  [Required(ErrorMessage = "FilterField is required")]
  public string filterField { get; init; } = "Title";

  [Required(ErrorMessage = "FilterValue is required")]
  public string filterValue { get; init; } = "ls";
}

public record SortParameters
{
  [Required(ErrorMessage = "SortField is required")]
  public string sortField { get; init; }

  [RegularExpression("asc|desc", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
  public string sortOrder { get; init; } = "asc";
}

public record GetExpenseListQueryParams : PaginationParameters
{
  public FilterParameters? Filter { get; init; }
  public SortParameters? Sort { get; init; }
}
