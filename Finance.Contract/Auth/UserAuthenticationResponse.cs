namespace Finance.Contract.Auth;

public record UserAuthenticationResponse(
  Guid id,
  string firstName,
  string lastName,
  string email
  );
