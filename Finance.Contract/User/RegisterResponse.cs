namespace Finance.Contract.Users;

public record RegisterResponse
{
  string firstName { get; init; } = null!;
  string lastName { get; init; } = null!;
  string email { get; init; } = null!;
  string token { get; init; } = null!;
}
