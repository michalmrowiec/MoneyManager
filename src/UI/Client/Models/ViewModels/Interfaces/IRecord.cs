using MoneyManager.Client.Models.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.ViewModels.Interfaces
{
    public interface IRecord : IId
    {
        //public int Id { get; set; }
        public decimal Amount { get; set; }

    }
}
