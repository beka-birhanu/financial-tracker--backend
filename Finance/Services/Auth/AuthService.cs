using ErrorOr;
using Finance.Data;
using Finance.Models;

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

  public Task<User> SignIn(Guid id)
  {
    throw new NotImplementedException();
  }
}
