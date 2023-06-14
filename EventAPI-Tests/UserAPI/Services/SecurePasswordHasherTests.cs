using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Services;

namespace EventAPI_Tests.UserAPI.Services
{
    public class SecurePasswordHasherTests
    {
        private SecurePasswordHasher _passwordHasher;

        [SetUp]
        public void Setup()
        {
            _passwordHasher = new SecurePasswordHasher();
        }

        [Test]
        public void HashPassword_ReturnsNonNullHashAndSalt()
        {
            // Arrange
            string password = "myPassword";

            // Act
            PasswordHash passwordHash = _passwordHasher.HashPasword(password);

            // Assert
            Assert.That(passwordHash.Hash, Is.Not.Null);
            Assert.That(passwordHash.Salt, Is.Not.Null);
        }

        [Test]
        public void VerifyPassword_WithValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "myPassword";
            PasswordHash passwordHash = _passwordHasher.HashPasword(password);

            // Act
            bool isPasswordValid = _passwordHasher.VerifyPassword(password, passwordHash.Hash, passwordHash.Salt);

            // Assert
            Assert.That(isPasswordValid, Is.True);
        }

        [Test]
        public void VerifyPassword_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            string password = "myPassword";
            PasswordHash passwordHash = _passwordHasher.HashPasword(password);
            string invalidPassword = "wrongPassword";

            // Act
            bool isPasswordValid = _passwordHasher.VerifyPassword(invalidPassword, passwordHash.Hash, passwordHash.Salt);

            // Assert
            Assert.That(isPasswordValid, Is.False);
        }
    }
}
