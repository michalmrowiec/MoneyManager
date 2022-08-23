using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils
{
    public class TestUtils
    {
        public static async Task PostItemsByListAsync<T>(HttpClient httpClient, IEnumerable<T> listOfItems, string uri)
        {
            foreach (var item in listOfItems)
            {
                await PostItemAsync(httpClient, item, uri);
            }
        }

        public static async Task PostItemAsync<T>(HttpClient httpClient, T item, string uri)
        {
            var json = JsonConvert.SerializeObject(item);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(uri, httpContent);
        }
    }
}
