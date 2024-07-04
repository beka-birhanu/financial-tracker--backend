using Finance.Contract.Auth;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Auth;
using Finance.Models;

namespace Finance.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost]
  [Route("signUp")]
  public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterationRequest request)
  {
    Models.User requestToUser = Models.User.From(request);
    await _authService.Register(requestToUser);

    return Ok(MapUserAuthenticationResponse(requestToUser));
  }

  [HttpPost]
  [Route("signIn")]
  public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
  {
    var signInResponse = await _authService.SignIn(request.email, request.password);

    return Ok(signInResponse);
  }

  private UserAuthenticationResponse MapUserAuthenticationResponse(User user)
  {
    return new UserAuthenticationResponse(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        "fkajsldkfjaskd"
        );
  }
}
