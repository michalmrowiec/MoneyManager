using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.Models.Response
{
    public class BaseResponse
    {
        public ResponseStatus Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? ValidationErrors { get; set; }

        public enum ResponseStatus
        {
            Success = 0,
            NotFound = 1,
            BadQuery = 2,
            ValidationError = 3
        }
    }
}
