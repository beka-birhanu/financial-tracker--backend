using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Auth;

public record UserRegistrationRequest(
  [Required(ErrorMessage = "firstName is required")]
  string firstName,

  [Required(ErrorMessage = "lastName is required")]
  string lastName,

  [RegularExpression(@"^[a-zA-Z0-9_.]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
  string email,

  [MinLength(8)]
  string password
);
