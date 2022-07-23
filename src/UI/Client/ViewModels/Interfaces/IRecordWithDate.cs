using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels.Interfaces
{
    public interface IRecordWithDate
    {
        DateTime Date { get; set; }
    }
}
