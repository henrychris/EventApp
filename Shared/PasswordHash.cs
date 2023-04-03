using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PasswordHash
    {
        public byte[] Salt { get; set; } = new byte[32];
        public string Hash { get; set; } = string.Empty;
    }
}
