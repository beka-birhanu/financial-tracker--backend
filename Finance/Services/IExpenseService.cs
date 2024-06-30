using ErrorOr;

namespace Finance.Services.Expenses;

public interface IExpenseService
{
  Created CreateExpense(Finance.Models.Expense expense);
  Finance.Models.Expense GetExpense(Guid id);
  List<Finance.Models.Expense> GetExpense();
  UpsertedExpense UpsertExpense(Finance.Models.Expense expense);
  Deleted DeleteExpense(Guid id);
}
