using ErrorOr;
using Finance.Contract.Expense;
using Finance.Errors;

namespace Finance.Models;

public class Expense
{
  public Guid Id { get; set; }
  public string Title { get; set; }
  public float Amount { get; set; }
  public DateTime Date { get; set; }


  private Expense(Guid id, string title, float amount, DateTime date)
  {
    Id = id;
    Title = title;
    Amount = amount;
    Date = date;
  }

  public static ErrorOr<Expense> From(CreateExpenseRequest request)
  {
    return Create(
        request.title,
        request.amount,
        request.date
        );
  }

  public static ErrorOr<Expense> From(Guid id, UpsertExpenseRequest request)
  {
    return Create(
        request.title,
        request.amount,
        request.date,
        id
        );
  }

  public static ErrorOr<Expense> Create(string title, float amount, DateTime date, Guid? id = null)
  {
    List<Error> errors = new();

    if (amount == 0)
    {
      errors.Add(ServiceError.ExpenseError.InvalidAmount);

      return errors;
    }

    return new Expense(
        id ?? Guid.NewGuid(),
        title,
        amount,
        date
        );
  }
}

