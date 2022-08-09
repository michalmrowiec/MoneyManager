using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Domain.Entities.Interfaces
{
    public interface IIdentifier
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
