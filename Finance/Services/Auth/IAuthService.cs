using ErrorOr;
using Finance.Models;

namespace Finance.Services.Auth;

public interface IAuthService
{
  public Task<Created> Register(User user);
  public Task<User> SignIn(Guid id);
}
