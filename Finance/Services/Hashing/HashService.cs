using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Finance.Services.Hashing;

public class HashService : IHashService
{
  private const int Iterations = 10000;
  private const int SaltSize = 16;
  private const int KeySize = 32;

  public string Hash(string word)
  {
    byte[] salt = new byte[SaltSize];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }

    byte[] hash = KeyDerivation.Pbkdf2(
        password: word,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: Iterations,
        numBytesRequested: KeySize);

    byte[] result = new byte[SaltSize + KeySize];
    Array.Copy(salt, 0, result, 0, SaltSize);
    Array.Copy(hash, 0, result, SaltSize, KeySize);

    return Convert.ToBase64String(result);

  }

  public bool Verify(string word, string hashedWord)
  {
    byte[] hashedPasswordBytes = Convert.FromBase64String(hashedWord);

    byte[] salt = new byte[SaltSize];
    Array.Copy(hashedPasswordBytes, 0, salt, 0, SaltSize);

    byte[] hash = KeyDerivation.Pbkdf2(
        password: word,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: Iterations,
        numBytesRequested: KeySize
    );

    for (int i = 0; i < KeySize; i++)
    {
      if (hashedPasswordBytes[SaltSize + i] != hash[i])
      {
        return false;
      }
    }

    return true;
  }
}
