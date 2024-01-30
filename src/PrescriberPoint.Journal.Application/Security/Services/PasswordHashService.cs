using System.Security.Cryptography;
using System.Text;

public interface IPasswordHashService {

    byte[] GenerateSalt(int keySize = 64);

    byte[] GetHash(string password, byte[] salt);
}

public class PasswordHashService : IPasswordHashService {
    const int iterations = 350000;
        
    public byte[] GenerateSalt(int keySize = 64) {

        var salt = RandomNumberGenerator.GetBytes(keySize);
        return salt;
    }

    public byte[] GetHash(string password, byte[] salt) {
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

         var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            salt.Length);

        return hash;
    }
}