namespace Finance.Contract.Expense;

public record GetExpenseResponse(
    int id,
    string title,
    int amount,
    DateTime date
    );
