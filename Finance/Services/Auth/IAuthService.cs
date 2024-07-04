using ErrorOr;
using Finance.Models;

namespace Finance.Services.Auth;

public interface IAuthService
{
  public Task<ErrorOr<Created>> Register(User user);
  public Task<ErrorOr<SignInResult>> SignIn(string email, string password);
}
