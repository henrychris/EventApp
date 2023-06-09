﻿using Shared;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Interfaces;

namespace UserAPI.Services
{
    public class SecurePasswordHasher : ISecurePasswordHasher
    {
        // set these in appsettings
        // refer to EP for how to categorise settings with classes
        const int _keySize = 128;
        const int _iterations = 350000;
        readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public PasswordHash HashPasword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _iterations,
                _hashAlgorithm,
                _keySize);

            return new PasswordHash
            {
                Hash = Convert.ToBase64String(hash),
                Salt = salt
            };
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash) || salt == null)
                return false;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _keySize);

            return hashToCompare.SequenceEqual(Convert.FromBase64String(hash));
        }
    }
}
