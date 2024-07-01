using Finance.Contract.Expense;

namespace Finance.Models;

public class Expense
{
  public Guid Id { get; }
  public string Title { get; }
  public float Amount { get; }
  public DateTime Date { get; }


  private Expense(Guid id, string title, float amount, DateTime date)
  {
    Id = id;
    Title = title;
    Amount = amount;
    Date = date;
  }

  public static Expense From(CreateExpenseRequest request)
  {
    return Create(
        request.title,
        request.amount,
        request.date
        );
  }

  public static Expense From(Guid id, UpsertExpenseRequest request)
  {
    return Create(
        request.title,
        request.amount,
        request.date,
        id
        );
  }

  public static Expense Create(string title, float amount, DateTime date, Guid? id = null)
  {
    return new Expense(
        id ?? Guid.NewGuid(),
        title,
        amount,
        date
        );
  }
}

