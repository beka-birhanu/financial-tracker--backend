using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Finance.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ErrorHandlingBaseController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)

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
