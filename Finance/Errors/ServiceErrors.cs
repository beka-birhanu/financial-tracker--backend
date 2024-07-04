using ErrorOr;

namespace Finance.Errors;

public static class ServiceError
{
  public static class ExpenseError
  {
    public static Error InvalidAmount => Error.Validation(
        code: "Expense.InvalidAmount",
        description: "Expense amount cannot be zero.");

    public static Error NotFound => Error.NotFound(
        code: "Expense.NotFound",
        description: "Expense not found.");
  }

  public static class UserError
  {
    public static Error EmailNotFound => Error.Validation(
        code: "User.EmailNotFound",
        description: "User with the provided email is not found.");

    public static Error EmailAlreadyUsed => Error.Validation(
        code: "User.EmailAlreadyUsed",
        description: "User with the provided email is already used.");

    public static Error NotFound => Error.Validation(
        code: "User.NotFound",
        description: "User not found.");
  }

  public static class AuthError
  {
    public static Error PasswordNotStrong => Error.Validation(
        code: "Auth.PasswordNotStrong",
        description: "The provided password is not strong enough.");

    public static Error PasswordNotCorrect => Error.Unauthorized(
        code: "Auth.PasswordNotCorrect",
        description: "The provided password is not correct.");
  }
}

