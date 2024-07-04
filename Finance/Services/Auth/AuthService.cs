using ErrorOr;
using Finance.Data;
using Finance.Models;
using Microsoft.EntityFrameworkCore;
using static Finance.Errors.ServiceError;

namespace Finance.Services.Auth;

public class AuthService : IAuthService
{
  private readonly UserContext _userContext;

  public AuthService(UserContext userContext)
  {
    _userContext = userContext;
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

    return MapToSignInResult(user);
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

  private SignInResult MapToSignInResult(User user)
  {
    return new SignInResult(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        "someAccessToken" // Replace with actual token generation logic
    );
  }
}

// using Finance.Contract.Users;
// using Finance.Data;
// using Finance.Models;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Threading.Tasks;
//
// public class UserService
// {
//   private readonly FinanceContext _context;
//
//   public UserService(FinanceContext context)
//   {
//     _context = context;
//   }
//
//   public async Task<ErrorOr<User>> RegisterUserAsync(RegisterRequest request)
//   {
//     var user = User.From(request);
//
//     try
//     {
//       _context.Users.Add(user);
//       await _context.SaveChangesAsync();
//       return user;
//     }
//     catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
//     {
//       // Handle unique constraint violation (error code 2601 for SQL Server)
//       return Error.Validation("Email", "Email is already in use.");
//     }
//   }
// }
