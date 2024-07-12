using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Auth;

public record UserRegistrationRequest(
  [Required(ErrorMessage = "firstName is required")]
  [RegularExpression(@"^[a-zA-Z0-9_]$", ErrorMessage = "Invalid firstName format")]
  string firstName,

  [Required(ErrorMessage = "lastName is required")]
  [RegularExpression(@"^[a-zA-Z0-9_]$", ErrorMessage = "Invalid lastName format")]
  string lastName,

  [RegularExpression(@"^[a-zA-Z0-9_.]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
  string email,

  [MinLength(8)]
  [RegularExpression(@"^[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Invalid password format")]
  string password
);
