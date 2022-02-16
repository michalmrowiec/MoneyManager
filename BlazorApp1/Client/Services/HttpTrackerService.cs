using BlazorApp.Server.Services;
using BlazorApp1.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BlazorApp1.Client.Services
{
    public interface IHttpTrackerService
    {
        Task<HttpResponseMessage> GetListOfItems(string uri);
        Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri);
        Task<HttpResponseMessage> DeleteItem(int id, string uri);
        Task<HttpResponseMessage> CreateItem<T>(T record, string uri);
        Task<HttpResponseMessage> UpdateItem<T>(T record, string uri) where T : IRecord;
    }
    public class HttpTrackerService : IHttpTrackerService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        public HttpTrackerService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<HttpResponseMessage> GetListOfItems(string uri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                var token = await _localStorage.GetItem<UserToken>("user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                return await _http.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> DeleteItem(int id, string uri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, uri + id))
            {
                var token = await _localStorage.GetItem<UserToken>("user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                return await _http.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> CreateItem<T>(T record, string uri)
        {
            var postJson = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json");
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                var token = await _localStorage.GetItem<UserToken>("user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                request.Content = postJson;
                return await _http.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> UpdateItem<T>(T record, string uri) where T : IRecord
        {
            var patchJson = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json");
            using (var request = new HttpRequestMessage(HttpMethod.Patch, uri))
            {
                var token = await _localStorage.GetItem<UserToken>("user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                request.Content = patchJson;
                return await _http.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri + categoryId))
            {
                var token = await _localStorage.GetItem<UserToken>("user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                return await _http.SendAsync(request);
            }
        }
    }
}
