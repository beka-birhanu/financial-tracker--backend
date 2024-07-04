using ErrorOr;
using Finance.Data;
using Finance.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Services.Auth;

public class AuthService : IAuthService
{
  private readonly UserContext _userContext;

  public AuthService(UserContext userContext)
  {
    _userContext = userContext;
  }

  public async Task<Created> Register(User user)
  {
    await _userContext.Users.AddAsync(user);
    await _userContext.SaveChangesAsync();

    return Result.Created;

  }

  public async Task<SignInResult> SignIn(string email, string password)
  {
    var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

    return MapToSignInResult(user);

  }

  private SignInResult MapToSignInResult(User user)
  {
    return new SignInResult(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        "kflasl;dkfjjaslk"
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
