namespace Finance.Contract.Expense;

public record UpsertExpenseRequest(
    int id,
    string title,
    int amount,
    DateTime date
    );
