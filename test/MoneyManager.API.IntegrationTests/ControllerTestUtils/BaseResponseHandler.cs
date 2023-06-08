using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils
{
    public class BaseResponseHandler
    {
        public async Task HandlerAsync(HttpResponseMessage response, string[] args)
        {
            var resString = await response.Content.ReadAsStringAsync();
            dynamic? baseResponse = JsonConvert.DeserializeObject<dynamic>(resString);
            bool success = Convert.ToBoolean(baseResponse?.success);
            int status = Convert.ToInt32(baseResponse?.status);
            List<string>? validationErrors = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(baseResponse?.validationErrors));
            
            var recordId = (int?)baseResponse?.recordId;

            List<object> argsValues = new();

            foreach (var arg in args)
            {
                argsValues.Add(baseResponse?.arg);
            }

        }
    }
}
