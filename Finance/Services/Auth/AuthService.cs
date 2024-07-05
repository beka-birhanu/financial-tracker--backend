using ErrorOr;
using Finance.Data;
using Finance.Models;
using Finance.Services.Hashing;
using Finance.Services.JWT;
using Microsoft.EntityFrameworkCore;
using static Finance.Errors.ServiceError;

namespace Finance.Services.Auth;

public class AuthService : IAuthService
{
  private readonly UserContext _userContext;
  private readonly IJwtService _jwtService;
  private readonly IHashService _hashService;

  public AuthService(UserContext userContext, IJwtService jwtService, IHashService hashService)
  {
    _userContext = userContext;
    _jwtService = jwtService;
    _hashService = hashService;
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

    user.Password = _hashService.Hash(user.Password);
    try
    {
      await _userContext.Users.AddAsync(user);
      await _userContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
    }

    return ErrorOr.Result.Created;
  }

  public async Task<ErrorOr<SignInResult>> SignIn(string email, string password)
  {
    var findUserResult = await FindUserByEmail(email);

    if (findUserResult.IsError)
    {
      return findUserResult.Errors;
    }

    User user = findUserResult.Value;

    if (!_hashService.Verify(password, user.Password))
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

