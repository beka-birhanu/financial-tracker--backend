using ErrorOr;
using Finance.Contract.Expense;
using Finance.Models;
using Finance.Services.Pagination;


namespace Finance.Services.Expenses;

public interface IExpenseService
{
  Task<ErrorOr<Created>> CreateExpenseAsync(Expense expense);
  Task<ErrorOr<Expense>> GetExpenseAsync(Guid id);
  Task<ErrorOr<PaginationResult<Expense>>> GetExpensesAsync(GetExpenseListQueryParams queryParams);
  Task<ErrorOr<UpsertedExpense>> UpsertExpenseAsync(Expense expense);
  Task<ErrorOr<Deleted>> DeleteExpenseAsync(Guid id);
}

