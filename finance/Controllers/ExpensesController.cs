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

  [HttpGet("{id: guid}")]
  public IActionResult GetExpense(Guid id)
  {
    return Ok("create");
  }

  [HttpPut("{id: guid}")]
  public IActionResult UpsertExpense(Guid id)
  {
    return Ok("create");
  }

  [HttpDelete("{id: guid}")]
  public IActionResult DeletExpense(Guid id)
  {
    return Ok("create");
  }
}
