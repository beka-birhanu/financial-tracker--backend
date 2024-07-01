using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Expense;

public record UpsertExpenseRequest(
    [Required(ErrorMessage = "Title is required.")]
    string title,

    [Range(0.01, float.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    float amount,

    [Required(ErrorMessage = "Date is required.")]
    DateTime date
);

