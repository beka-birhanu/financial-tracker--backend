using ErrorOr;
using Finance.Data;
using Finance.Models;
using Finance.Services.JWT;
using Microsoft.EntityFrameworkCore;
using static Finance.Errors.ServiceError;

namespace Finance.Services.Auth;

public class AuthService : IAuthService
{
  private readonly UserContext _userContext;
  private readonly IJwtService _jwtService;

  public AuthService(UserContext userContext, IJwtService jwtService)
  {
    _userContext = userContext;
    _jwtService = jwtService;
  }

  public async Task<ErrorOr<Created>> Register(User user)
  {
    var findExistingUserResult = await FindUserByEmail(user.Email);

    if (!findExistingUserResult.IsError)
    {
      return UserError.EmailAlreadyUsed;
    }

    if (!CheckPasswordStrength(user.Password))
    {
      return AuthError.PasswordNotStrong;
    }

    try
    {
      await _userContext.Users.AddAsync(user);
      await _userContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
    }

    return Result.Created;
  }

  public async Task<ErrorOr<SignInResult>> SignIn(string email, string password)
  {
    var findUserResult = await FindUserByEmail(email);

    if (findUserResult.IsError)
    {
      return findUserResult.Errors;
    }

    User user = findUserResult.Value;

    if (!CheckPasswordCorrectness(password, user.Password))
    {
      return AuthError.PasswordNotCorrect;
    }

    string token = _jwtService.SignToken(user.Id);
    return MapToSignInResult(user, token);
  }

  private async Task<ErrorOr<User>> FindUserByEmail(string email)
  {
    var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

    if (user == null)
    {
      return UserError.EmailNotFound;
    }

    return user;
  }

  private bool CheckPasswordCorrectness(string password, string hashedPassword)
  {
    // Implement actual password comparison logic here
    return password == hashedPassword;
  }

  private bool CheckPasswordStrength(string password)
  {
    return password.Length >= 12;
  }

  private SignInResult MapToSignInResult(User user, string token)
  {
    return new SignInResult(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        token
    );
  }
}

