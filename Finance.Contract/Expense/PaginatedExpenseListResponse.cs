namespace Finance.Contract.Expense;


public record PaginatedExpenseListResponse(
    List<ExpenseResponse> expenses,
    int totalCount,
    int pageNumber,
    int pageSize
);

