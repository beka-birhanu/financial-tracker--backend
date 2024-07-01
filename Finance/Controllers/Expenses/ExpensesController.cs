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
  private readonly IExpenseService _expenseServiece;

  public ExpensesController(IExpenseService expenseServiece)
  {
    _expenseServiece = expenseServiece;
  }

  [HttpPost]
  public IActionResult CreateExpense(CreateExpenseRequest request)
  {
    ErrorOr<Expense> requestToExpenseResponse = Expense.From(request);

    if (requestToExpenseResponse.IsError)
    {
      return Problem(requestToExpenseResponse.Errors);
    }

    Expense expense = requestToExpenseResponse.Value;
    ErrorOr<Created> createExpenseResponse = _expenseServiece.CreateExpense(expense);


    return createExpenseResponse.Match(
        created => CreatedAtGetExpense(expense),
        errors => Problem(errors)
        );
  }

  [HttpGet]
  public IActionResult GetExpenseList()
  {
    ErrorOr<List<Expense>> getExpenseResult = _expenseServiece.GetExpense();

    return getExpenseResult.Match(
        expenses => Ok(MapExpenseListResponse(expenses)),
        errors => Problem(errors)
        );
  }


  [HttpGet("{id:guid}")]
  public IActionResult GetExpense(Guid id)
  {
    ErrorOr<Expense> getExpenseResult = _expenseServiece.GetExpense(id);

    return getExpenseResult.Match(
        expense => Ok(MapExpenseResponse(expense)),
        errors => Problem(errors)
        );
  }

  [HttpPut("{id:guid}")]
  public IActionResult UpsertExpense(Guid id, UpsertExpenseRequest request)
  {
    ErrorOr<Expense> upsertExpenseResponse = Expense.From(id, request);

    if (upsertExpenseResponse.IsError)
    {
      return Problem(upsertExpenseResponse.Errors);
    }

    Expense expense = upsertExpenseResponse.Value;
    ErrorOr<UpsertedExpense> upsertedExpenseResponse = _expenseServiece.UpsertExpense(expense);

    return upsertedExpenseResponse.Match(
        upserted => upserted.IsNewlyCreated ? CreatedAtGetExpense(expense) : NoContent(),
        errors => Problem(errors)
        );
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeletExpense(Guid id)
  {
    ErrorOr<Deleted> deleteExpenseResponse = _expenseServiece.DeleteExpense(id);
    return deleteExpenseResponse.Match(
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
        actionName: nameof(GetExpense),
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
