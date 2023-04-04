using Shared;

namespace UserAPI.Interfaces
{
    public interface ISecurePasswordHasher
    {
        public PasswordHash HashPasword(string password, out byte[] salt);
        public bool VerifyPassword(string password, string hash, byte[] salt);
    }
}
