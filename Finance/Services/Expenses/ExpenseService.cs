using ErrorOr;
using Finance.Contract.Expense;
using Finance.Data;
using Finance.Errors;
using Finance.Models;
using Finance.Services.FilterSort;
using Finance.Services.Pagination;


namespace Finance.Services.Expenses;

public class ExpenseService : IExpenseService
{
  private readonly ExpenseContext _expenseContext;
  private readonly IPaginationStrategy<Expense> _paginationStrategy;
  private readonly IFilterSortStrategy<Expense> _filterSortStrategy;

  public ExpenseService(ExpenseContext expenseContext, IPaginationStrategy<Expense> paginagionStrategy, IFilterSortStrategy<Expense> filterSortStrategy)
  {
    _expenseContext = expenseContext;
    _paginationStrategy = paginagionStrategy;
    _filterSortStrategy = filterSortStrategy;
  }

  public async Task<ErrorOr<Created>> CreateExpenseAsync(Expense expense)
  {
    await _expenseContext.Expenses.AddAsync(expense);
    await _expenseContext.SaveChangesAsync();

    return Result.Created;
  }

  public async Task<ErrorOr<Expense>> GetExpenseAsync(Guid id)
  {
    var expense = await _expenseContext.Expenses.FindAsync(id);
    if (expense is null)
    {
      return ServiceError.Expense.NotFound;
    }

    return expense;
  }

  public async Task<ErrorOr<PaginationResult<Expense>>> GetExpensesAsync(GetExpenseListQueryParams queryParams)
  {
    var query = _expenseContext.Expenses.AsQueryable();

    Console.WriteLine(queryParams);
    if (queryParams.Filter != null)
    {
      query = _filterSortStrategy.ApplyFilter(query, queryParams.Filter.filterValue, queryParams.Filter.filterField);
    }

    if (queryParams.Sort != null)
    {
      query = _filterSortStrategy.ApplySort(query, queryParams.Sort.sortOrder, queryParams.Sort.sortField);
    }
    else
    {
      // Default sort
      query = _filterSortStrategy.ApplySort(query, "asc", "Date");
    }

    PaginationResult<Expense> paginatedResult = await _paginationStrategy.PaginateAsync(query, queryParams.pageNumber, queryParams.pageSize);

    return paginatedResult;
  }


  public async Task<ErrorOr<UpsertedExpense>> UpsertExpenseAsync(Expense expense)
  {
    var existingExpense = await _expenseContext.Expenses.FindAsync(expense.Id);
    var isNewlyCreated = existingExpense is null;

    if (isNewlyCreated)
    {
      await _expenseContext.Expenses.AddAsync(expense);
    }
    else
    {
      _expenseContext.Entry(existingExpense).CurrentValues.SetValues(expense);
    }

    await _expenseContext.SaveChangesAsync();
    return new UpsertedExpense(isNewlyCreated);
  }

  public async Task<ErrorOr<Deleted>> DeleteExpenseAsync(Guid id)
  {
    var expense = await _expenseContext.Expenses.FindAsync(id);
    if (expense is null)
    {
      return ServiceError.Expense.NotFound;
    }

    _expenseContext.Expenses.Remove(expense);
    await _expenseContext.SaveChangesAsync();

    return Result.Deleted;
  }
}

