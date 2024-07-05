namespace Finance.Services.JWT;

public interface IJwtService
{
  public string SignToken(Guid id);
}

