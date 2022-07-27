using MoneyManager.Client.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels
{
    public class RecordVM : IRecord, IRecordWithDate
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Name is too short.")]
        [StringLength(25,ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = null!;
        public string? CategoryName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// Transaction Date
        /// </summary>
        public DateTime TransactionDate { get; set; }
        public int? CategoryId { get; set; }
    }
}
