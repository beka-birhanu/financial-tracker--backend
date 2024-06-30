namespace Finance.Contract.Expense;

public record UpsertExpenseRequest(
    Guid id,
    string title,
    int amount,
    DateTime date
    );
