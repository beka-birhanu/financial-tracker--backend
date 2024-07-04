using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Users;

public record RegisterRequest(
  [Required(ErrorMessage = "firstName is required")]
  string firstName,

  [Required(ErrorMessage = "lastName is required")]
  string lastName,

  [RegularExpression("/^[a-zA-Z0-9_.]+$/", ErrorMessage = "Invalid email format")]
  string email,

  [MinLength(8)]
  string password
);
