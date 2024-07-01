using Finance.Contract.Expense;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Expenses;
using Finance.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Finance.Controllers.Expenses;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
  private readonly IExpenseService _expenseService;

  public ExpensesController(IExpenseService expenseService)
  {
    _expenseService = expenseService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateExpenseAsync(CreateExpenseRequest request)
  {
    ErrorOr<Expense> requestToExpenseResult = Expense.From(request);

    if (requestToExpenseResult.IsError)
    {
      return Problem(requestToExpenseResult.Errors);
    }

    Expense expense = requestToExpenseResult.Value;
    ErrorOr<Created> createExpenseResult = await _expenseService.CreateExpenseAsync(expense);

    return createExpenseResult.Match(
        created => CreatedAtGetExpense(expense),
        errors => Problem(errors)
    );
  }

  [HttpGet]
  public async Task<IActionResult> GetExpenseListAsync()
  {
    ErrorOr<List<Expense>> getExpenseResult = await _expenseService.GetExpensesAsync();

    return getExpenseResult.Match(
        expenses => Ok(MapExpenseListResponse(expenses)),
        errors => Problem(errors)
    );
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetExpenseAsync(Guid id)
  {
    ErrorOr<Expense> getExpenseResult = await _expenseService.GetExpenseAsync(id);

    return getExpenseResult.Match(
        expense => Ok(MapExpenseResponse(expense)),
        errors => Problem(errors)
    );
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpsertExpenseAsync(Guid id, UpsertExpenseRequest request)
  {
    ErrorOr<Expense> upsertExpenseResult = Expense.From(id, request);

    if (upsertExpenseResult.IsError)
    {
      return Problem(upsertExpenseResult.Errors);
    }

    Expense expense = upsertExpenseResult.Value;
    ErrorOr<UpsertedExpense> upsertedExpenseResult = await _expenseService.UpsertExpenseAsync(expense);

    return upsertedExpenseResult.Match(
        upserted => upserted.IsNewlyCreated ? CreatedAtGetExpense(expense) : NoContent(),
        errors => Problem(errors)
    );
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteExpenseAsync(Guid id)
  {
    ErrorOr<Deleted> deleteExpenseResult = await _expenseService.DeleteExpenseAsync(id);

    return deleteExpenseResult.Match(
        deleted => NoContent(),
        errors => Problem(errors)
    );
  }

  private ExpenseListResponse MapExpenseListResponse(List<Expense> expenses)
  {
    return new ExpenseListResponse(expenses.Select(MapExpenseResponse).ToList());
  }

  private static ExpenseResponse MapExpenseResponse(Expense expense)
  {
    return new ExpenseResponse(
        expense.Id,
        expense.Title,
        expense.Amount,
        expense.Date
    );
  }

  private CreatedAtActionResult CreatedAtGetExpense(Expense expense)
  {
    return CreatedAtAction(
        actionName: nameof(GetExpenseAsync),
        routeValues: new { id = expense.Id },
        value: MapExpenseResponse(expense)
    );
  }

  private IActionResult Problem(List<Error> errors)
  {
    if (errors.All(e => e.Type == ErrorType.Validation))
    {
      ModelStateDictionary modelStateDictionary = new();

      foreach (Error error in errors)
      {
        modelStateDictionary.AddModelError(error.Code, error.Description);
      }

      return ValidationProblem(modelStateDictionary);
    }

    if (errors.Any(e => e.Type == ErrorType.Unexpected))
    {
      return Problem();
    }

    Error firstError = errors[0];

    var statusCode = firstError.Type switch
    {
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      _ => StatusCodes.Status500InternalServerError
    };

    return Problem(statusCode: statusCode, title: firstError.Description);
  }
}

