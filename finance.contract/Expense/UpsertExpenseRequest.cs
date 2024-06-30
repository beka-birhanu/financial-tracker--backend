namespace Finance.Contract.Expense;

public record UpsertExpenseRequest(
    string title,
    int amount,
    DateTime date
    );
