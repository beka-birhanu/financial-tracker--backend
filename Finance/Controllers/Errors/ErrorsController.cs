using Microsoft.AspNetCore.Mvc;

namespace Finance.Controllers.Errors;

[ApiController]
[Route("error")]
public class ErrorsContrller : ControllerBase
{
  [HttpGet]
  public IActionResult Index()
  {
    return Problem();
  }
}
