using Finance.Contract.Auth;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Auth;
using ErrorOr;
using SignInResult = Finance.Services.Auth.SignInResult;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Finance.Controllers.Auth;

public class AuthController : ErrorHandlingBaseController
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("signUp")]
  public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest request)
  {
    Models.User requestToUser = Models.User.From(request);

    ErrorOr<Created> registerResult = await _authService.Register(requestToUser);

    if (registerResult.IsError)
    {
      return Problem(registerResult.Errors);
    }

    var signInResponse = await _authService.SignIn(request.email, request.password);

    return signInResponse.Match(
        signInData => Ok(MapUserAuthenticationResponse(signInData)),
        errors => Problem(errors)
    );
  }

  [HttpPost("signIn")]
  public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
  {
    var signInResponse = await _authService.SignIn(request.email, request.password);

    if (signInResponse.IsError)
    {
      return Problem(signInResponse.Errors);
    }

    var signInResult = signInResponse.Value;
    UserAuthenticationResponse authResponse = MapUserAuthenticationResponse(signInResult);

    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict
    };

    Response.Cookies.Append("access-token", signInResult.Token, cookieOptions);

    return Ok(authResponse);
  }

  private UserAuthenticationResponse MapUserAuthenticationResponse(SignInResult result)
  {
    return new UserAuthenticationResponse(
        result.Id,
        result.FirstName,
        result.LastName,
        result.Email
    );
  }

}

