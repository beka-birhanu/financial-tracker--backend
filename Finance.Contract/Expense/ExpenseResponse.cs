namespace Finance.Contract.Expense;

public record ExpenseResponse(
    Guid id,
    string title,
    float amount,
    DateTime date
    );
