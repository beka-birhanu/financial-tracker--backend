using System.ComponentModel.DataAnnotations;

namespace Finance.Contract.Users;

public record RegisterRequest
{
  [Required(ErrorMessage = "firstName is required")]
  string firstName { get; init; } = null!;

  [Required(ErrorMessage = "lastName is required")]
  string lastName { get; init; } = null!;

  [RegularExpression("/^[a-zA-Z0-9_.]+$/", ErrorMessage = "Invalid email format")]
  string email { get; init; } = null!;

  [MinLength(8)]
  string password { get; init; } = null!;
}
