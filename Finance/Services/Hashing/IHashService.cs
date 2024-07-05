namespace Finance.Services.Hashing;

public interface IHashService
{
  public string Hash(string word);
  public bool Verify(string word, string hashedWord);
}

