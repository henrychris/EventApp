namespace Shared
{
    public class PasswordHash
    {
        public byte[] Salt { get; set; } = new byte[32];
        public string Hash { get; set; } = string.Empty;
    }
}
