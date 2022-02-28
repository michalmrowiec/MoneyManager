using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Domain.Authentication
{
    public class RegisterUser
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RepeatPassword { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
