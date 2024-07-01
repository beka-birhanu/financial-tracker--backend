using ErrorOr;
using Finance.Models;


namespace Finance.Services.Expenses;

public interface IExpenseService
{
  Task<ErrorOr<Created>> CreateExpenseAsync(Expense expense);
  Task<ErrorOr<Expense>> GetExpenseAsync(Guid id);
  Task<ErrorOr<List<Expense>>> GetExpensesAsync();
  Task<ErrorOr<UpsertedExpense>> UpsertExpenseAsync(Expense expense);
  Task<ErrorOr<Deleted>> DeleteExpenseAsync(Guid id);
}

