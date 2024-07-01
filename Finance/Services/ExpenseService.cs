using ErrorOr;
using Finance.Errors;
using Finance.Models;

namespace Finance.Services.Expenses;

public class ExpenseService : IExpenseService
{
  private static readonly Dictionary<Guid, Expense> _expenses = new();

  public ErrorOr<Created> CreateExpense(Expense expense)
  {
    _expenses.Add(expense.Id, expense);

    return Result.Created;
  }

  public ErrorOr<Expense> GetExpense(Guid id)
  {
    if (_expenses.TryGetValue(id, out var expense))
    {
      return expense;
    }

    return ServiceError.Expense.NotFound;
  }

  public ErrorOr<List<Expense>> GetExpense()
  {
    return _expenses.Values.ToList();
  }

  public ErrorOr<UpsertedExpense> UpsertExpense(Expense expense)
  {
    var isNewlyCreated = !_expenses.ContainsKey(expense.Id);
    _expenses[expense.Id] = expense;

    return new UpsertedExpense(isNewlyCreated);
  }

  public ErrorOr<Deleted> DeleteExpense(Guid id)
  {
    if (!_expenses.ContainsKey(id))
    {
      return ServiceError.Expense.NotFound;
    }
    _expenses.Remove(id);

    return Result.Deleted;
  }

}
