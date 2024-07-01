using ErrorOr;
using Finance.Models;

namespace Finance.Services.Expenses;

public interface IExpenseService
{
  Created CreateExpense(Expense expense);
  Expense GetExpense(Guid id);
  List<Expense> GetExpense();
  UpsertedExpense UpsertExpense(Expense expense);
  Deleted DeleteExpense(Guid id);
}
