using Finance.Contract.Expense;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers;

public class ExpenseController : ControllerBase
{

  [HttpPost]
  public IActionResult CreateExpense(CreateExpenseRequest request)
  {
    return Ok("create");
  }

  [HttpGet]
  public IActionResult GetExpenseList()
  {
    return Ok("create");
  }

  [HttpGet("{id: int}")]
  public IActionResult GetExpense()
  {
    return Ok("create");
  }

  [HttpPut("{id: int}")]
  public IActionResult UpsertExpense()
  {
    return Ok("create");
  }

  [HttpDelete("{id: int}")]
  public IActionResult DeletExpense()
  {
    return Ok("create");
  }
}
