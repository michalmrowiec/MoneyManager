using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Client.ViewModels
{
    public class RegisterUserVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        [Required]
        [MinLength(6)]
        public string RepeatPassword { get; set; } = null!;
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = null!;
    }
}
