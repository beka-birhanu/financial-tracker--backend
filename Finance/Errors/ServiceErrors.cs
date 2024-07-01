using ErrorOr;

namespace Finance.Errors;

public static class ServiceError
{
  public static class Expense
  {
    public static Error InvalidAmount => Error.Validation(
      code: "Expense.InvalidName",
      description: $"Expense amount cannot be zero.");

    public static Error NotFound => Error.NotFound(
        code: "Expense.NotFound",
        description: "Expense not found");
  }
}
