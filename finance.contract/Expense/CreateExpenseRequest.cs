namespace Finance.Contract.Expense;

public record CreateExpenseRequest(
    string title,
    int amount,
    DateTime date
    );
