namespace Finance.Contract.Expense;

public record CreateExpenseRequest(
    string title,
    float amount,
    DateTime date
    );
