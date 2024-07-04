namespace Finance.Contract.Auth;

public record SignInRequest(
    string email,
    string password);
