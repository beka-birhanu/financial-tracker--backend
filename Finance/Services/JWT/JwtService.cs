using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Services.JWT
{
  public class JwtService : IJwtService
  {
    private readonly IConfiguration _configuration;
    private readonly string _jwtKey;

    public JwtService(IConfiguration configuration)
    {
      _configuration = configuration;
      _jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("JWT key is not configured");
    }

    public string SignToken(Guid id)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_jwtKey);

      var tokenDescriptor = GetTokenDescriptor(id, key);
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(Guid id, byte[] key)
    {
      return new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
          {
                    new Claim("Id", id.ToString())
                }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
    }
  }
}

