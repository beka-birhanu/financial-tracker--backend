using ErrorOr;
using Finance.Data;
using Finance.Errors;
using Finance.Models;
using Microsoft.EntityFrameworkCore;


namespace Finance.Services.Expenses;
public class ExpenseService : IExpenseService
{
  private readonly ExpenseContext _expenseContext;

  public ExpenseService(ExpenseContext expenseContext)
  {
    _expenseContext = expenseContext;
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

  public async Task<ErrorOr<List<Expense>>> GetExpensesAsync()
  {
    return await _expenseContext.Expenses.ToListAsync();
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

