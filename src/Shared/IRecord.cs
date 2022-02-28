using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Shared
{
    public interface IRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
