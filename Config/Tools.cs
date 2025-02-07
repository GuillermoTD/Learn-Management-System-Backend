using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;

namespace Learn_Managment_System_Backend.Tools
{
    public class Tools
    {

        //se encripta la contraseña pasada por parametro
        public static string EncryptPassword(string password)
        {
            using var sha256 = SHA256.Create();
            //se crea un arreglo de bytes a partir del string de la contraseña
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            //Dichos bytes son convertidos a string  y dicho string representa la contraseña que se almacena en base de datos    
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }



        public static bool CheckIfPasswordIsValid(string password, string hashedPassword)
        {
             private  int SaltSize = 16; // 16 bytes = 128 bits
    private  int KeySize = 32; // 32 bytes = 256 bits
    private  int Iterations = 100000; // Ajusta según necesidad
    // private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;
            var parts = hashedPassword.Split('.');
            
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithm,
                KeySize);

            return CryptographicOperations.FixedTimeEquals(newHash, hash);
        }


        //     public static string CheckIfPasswordIsValid(string password, string hashedPassword)
        //     {

        //         var parts = hashedPassword.Split('.');
        //         private const int SaltSize = 16; // 16 bytes = 128 bits
        //     private const int KeySize = 32; // 32 bytes = 256 bits
        //     private const int Iterations = 100000; // Ajusta según necesidad
        //     private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        //         if (parts.Length != 2)
        //             {
        //                 return false;
        //             }

        //         //salt
        //         byte[] salt = Convert.FromBase64String(parts[0]);
        //     //Hashed
        //     byte[] hash = Convert.FromBase64String(parts[1]);

        //     byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(
        //     Encoding.UTF8.GetBytes(password),
        //         salt,
        //         Iterations,
        //         HashAlgorithm,
        //         KeySize
        // );


        // }

    }



}

