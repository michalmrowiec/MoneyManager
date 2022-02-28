using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Shared
{
    public class CategoryItemDto : IRecord
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Category name is too long.")]
        public string Name { get; set; } = null!;
    }
}
