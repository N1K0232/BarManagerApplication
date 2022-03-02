using BackendGestionaleBar.Security.Models.Request;
using BackendGestionaleBar.Security.Models.Response;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace BackendGestionaleBar.Security
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public CheckPasswordResponse Check(CheckPasswordRequest request)
        {
            string[] parts = request.Hash.Split(',', 3);
            if (parts.Length != 3)
            {
                throw new FormatException("Formato della stringa non corretto");
            }

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(request.Password, salt, iterations, HashAlgorithmName.SHA512);
            byte[] keyToCheck = algorithm.GetBytes(KeySize);
            bool verified = keyToCheck.SequenceEqual(key);
            bool needsUpgrade = iterations != Iterations;

            return new CheckPasswordResponse(verified, needsUpgrade);
        }
        public string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);
            return $"{Iterations}.{salt}.{key}";
        }
    }
}