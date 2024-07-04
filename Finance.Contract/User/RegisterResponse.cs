namespace Finance.Contract.Users;

public record RegisterResponse(
  Guid id,
  string firstName,
  string lastName,
  string email,
  string token
  );
