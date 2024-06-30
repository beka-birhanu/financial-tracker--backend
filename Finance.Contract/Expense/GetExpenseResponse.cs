namespace Finance.Contract.Expense;

public record GetExpenseResponse(
    Guid id,
    string title,
    int amount,
    DateTime date
    );
