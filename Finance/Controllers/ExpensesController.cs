using Finance.Contract.Expense;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Expenses;
using Finance.Models;

namespace Finance.Controllers;

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
    Expense expense = Expense.From(request);
    _expenseServiece.CreateExpense(expense);

    return CreatedAtGetExpense(expense);
  }

  [HttpGet]
  public IActionResult GetExpenseList()
  {
    List<Expense> expenses = _expenseServiece.GetExpense();
    ExpenseListResponse expensesResponse = MapExpenseListResponse(expenses);

    return Ok(expensesResponse);
  }


  [HttpGet("{id:guid}")]
  public IActionResult GetExpense(Guid id)
  {
    Expense expense = _expenseServiece.GetExpense(id);

    return Ok(MapExpenseResponse(expense));
  }

  [HttpPut("{id:guid}")]
  public IActionResult UpsertExpense(Guid id, UpsertExpenseRequest request)
  {
    Expense expense = Expense.From(id, request);
    UpsertedExpense upsertedExpenseResponse = _expenseServiece.UpsertExpense(expense);

    return upsertedExpenseResponse.IsNewlyCreated ? CreatedAtGetExpense(expense) : NoContent();
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeletExpense(Guid id)
  {
    _expenseServiece.DeleteExpense(id);
    return NoContent();
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
}
