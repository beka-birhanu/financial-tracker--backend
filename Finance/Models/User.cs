using Finance.Contract.Auth;

namespace Finance.Models;

public class User
{
  public Guid Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }

  private User(Guid id, string firstName, string lastName, string email, string password)
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
    Email = email;
    Password = password;
  }

  public static User From(UserRegistrationRequest request)
  {
    // to do hash password
    string hashedPassword = request.password;

    return new User(
        Guid.NewGuid(),
        request.firstName,
        request.lastName,
        request.email,
        hashedPassword
        );

  }
}


