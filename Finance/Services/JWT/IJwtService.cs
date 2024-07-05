using ErrorOr;

namespace Finance.Services.JWT;

public interface IJwtService
{
  public string SignToken(Guid id);
  public ErrorOr<string> Decode(string token);
}

