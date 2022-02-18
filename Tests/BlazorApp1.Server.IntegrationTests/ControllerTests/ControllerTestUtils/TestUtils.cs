using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Server.IntegrationTests.ControllerTests.ControllerTestUtils
{
    public static class TestUtils
    {
        public static async Task PostRecordsByList<T>(HttpClient httpClient, IEnumerable<T> listOfItems, string uri)
        {
            foreach (var item in listOfItems)
            {
                var json = JsonConvert.SerializeObject(item);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                await httpClient.PostAsync(uri, httpContent);
            }
        }
    }
}
