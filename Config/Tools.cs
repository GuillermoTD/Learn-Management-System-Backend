using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;

namespace Learn_Managment_System_Backend.Config
{
    public static class Tools
    {
        private const int SaltSize = 16; // 16 bytes = 128 bits
        private const int KeySize = 32; // 32 bytes = 256 bits
        private const int Iterations = 100000;
        private static readonly KeyDerivationPrf HashAlgorithm = KeyDerivationPrf.HMACSHA256;

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

            // Aqui se devuelve un arreglo de bytes que representan la contraseña
            byte[] hashedBytes = KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            /*Aqui se devuelve la contraseña dividida por el salt, un punto
            y luego la contraseña. Esto es util al momento de validar la contraseña,
            si la contraseña no cumple con este orden entonces no es correcta. */
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashedBytes)}";

        }


        public static bool CheckIfPasswordIsValid(string plainPassword, string hashedPasswordFromDataBase)
        {
            var parts = hashedPasswordFromDataBase.Split('.');

            /*Validacion del formato de la contraseña ingresada creado en la funcion de
            hasheo, sino cumple con ese formato entonces la contraseña no es valida.*/
            if (parts.Length != 2)
                return false;

            /*Aqui se dividen las partes de la contraseña conviertiendolas de base64 a string*/
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            /*Aqui se calcula la contraseña*/
            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: plainPassword,
                salt: salt,
                prf: HashAlgorithm,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            /*Se compara el hash generado con el hash que esta en la base de datos*/
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }



}

