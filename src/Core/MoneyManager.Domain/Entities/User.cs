using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Record>? Records { get; set;}
        public virtual ICollection<Category>? Categories { get; set;}
        public virtual ICollection<RecurringRecord>? RecurringRecords { get; set;}
    }
}
