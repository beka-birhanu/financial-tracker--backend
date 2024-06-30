using Finance.Contract.Expense;

namespace Finance.Models;

public class Expense
{
  public Guid Id { get; }
  public string Title { get; }
  public int Amount { get; }
  public DateTime Date { get; }


  private Expense(Guid id, string title, int amount, DateTime date)
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

  public static Expense From(UpsertExpenseRequest request)
  {
    return Create(
        request.title,
        request.amount,
        request.date,
        request.id
        );
  }

  public static Expense Create(string title, int amount, DateTime date, Guid? id = null)
  {
    return new Expense(
        id ?? Guid.NewGuid(),
        title,
        amount,
        date
        );
  }
}

