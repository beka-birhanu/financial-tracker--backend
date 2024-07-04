using ErrorOr;
using Finance.Models;

namespace Finance.Services.Auth;

public interface IAuthService
{
  public Task<Created> Register(User user);
  public Task<SignInResult> SignIn(string email, string password);
}
