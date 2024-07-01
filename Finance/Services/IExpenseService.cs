using ErrorOr;
using Finance.Models;

namespace Finance.Services.Expenses;

public interface IExpenseService
{
  ErrorOr<Created> CreateExpense(Expense expense);
  ErrorOr<Expense> GetExpense(Guid id);
  ErrorOr<List<Expense>> GetExpense();
  ErrorOr<UpsertedExpense> UpsertExpense(Expense expense);
  ErrorOr<Deleted> DeleteExpense(Guid id);
}
