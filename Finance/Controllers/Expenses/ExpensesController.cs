using Finance.Contract.Expense;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Expenses;
using Finance.Models;
using ErrorOr;
using Finance.Services.Pagination;

namespace Finance.Controllers.Expenses;

public class ExpensesController : ApiController
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
  public async Task<IActionResult> GetExpenseListAsync([FromQuery] GetExpenseListQueryParams queryParams)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    ErrorOr<PaginationResult<Expense>> getPaginatedExpenseResult = await _expenseService.GetExpensesAsync(queryParams);

    return getPaginatedExpenseResult.Match(
        paginatedResult => Ok(MapPaginatedExpenseListResponse(paginatedResult)),
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

  private PaginatedExpenseListResponse MapPaginatedExpenseListResponse(PaginationResult<Expense> paginatedExpenses)
  {
    List<ExpenseResponse> expenses = paginatedExpenses.Data
        .Select(expense => MapExpenseResponse(expense))
        .ToList();

    return new PaginatedExpenseListResponse
      (expenses,
       paginatedExpenses.TotalCount,
       paginatedExpenses.PageNumber,
       paginatedExpenses.PageSize)
    ;
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
}

