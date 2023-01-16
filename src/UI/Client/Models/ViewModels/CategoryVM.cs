using MoneyManager.Client.Models.ViewModels.Interfaces;
using MoneyManager.Client.ViewModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Client.ViewModels
{
    public class CategoryVM : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Category name is too long.")]
        public string Name { get; set; } = null!;
    }
}
