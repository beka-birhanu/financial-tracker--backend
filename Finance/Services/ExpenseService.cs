using ErrorOr;
using Finance.Models;

namespace Finance.Services.Expenses;

public class ExpenseService : IExpenseService
{
  private static readonly Dictionary<Guid, Expense> _expenses = new();

  public Created CreateExpense(Expense expense)
  {
    _expenses.Add(expense.Id, expense);

    return Result.Created;
  }

  public Expense GetExpense(Guid id)
  {

    return _expenses[id];
  }

  public List<Expense> GetExpense()
  {
    return _expenses.Values.ToList();
  }

  public UpsertedExpense UpsertExpense(Expense expense)
  {
    var isNewlyCreated = !_expenses.ContainsKey(expense.Id);
    _expenses[expense.Id] = expense;

    return new UpsertedExpense(isNewlyCreated);
  }

  public Deleted DeleteExpense(Guid id)
  {
    _expenses.Remove(id);

    return Result.Deleted;
  }

}
