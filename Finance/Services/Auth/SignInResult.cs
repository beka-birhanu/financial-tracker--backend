namespace Finance.Services.Auth;

public record SignInResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token
    );


