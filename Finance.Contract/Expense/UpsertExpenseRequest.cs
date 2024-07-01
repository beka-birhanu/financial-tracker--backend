namespace Finance.Contract.Expense;

public record UpsertExpenseRequest(
    string title,
    float amount,
    DateTime date
    );
