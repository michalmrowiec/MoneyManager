﻿using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Client.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
