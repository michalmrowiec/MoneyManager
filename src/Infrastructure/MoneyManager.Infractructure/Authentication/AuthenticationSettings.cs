using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Authentication
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; } = null!;
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; } = null!;
    }
}
