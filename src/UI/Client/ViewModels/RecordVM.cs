using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class RecordVM : IRecord
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
    }
}
