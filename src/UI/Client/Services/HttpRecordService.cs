﻿using MoneyManager.Client.Models.ViewModels.Interfaces;
using MoneyManager.Client.ViewModels;
using MoneyManager.Client.ViewModels.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MoneyManager.Client.Services
{
    public interface IHttpRecordService
    {
        Task<HttpResponseMessage> GetListOfItems(string uri);
        Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri);
        Task<HttpResponseMessage> DeleteItem(int id, string uri);
        Task<HttpResponseMessage> CreateItem<T>(T record, string uri);
        Task<HttpResponseMessage> UpdateItem<T>(T record, string uri, string? token = null);
    }
    public class HttpRecordService : IHttpRecordService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        public HttpRecordService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<HttpResponseMessage> GetListOfItems(string uri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var token = await _localStorage.GetItem<UserTokenVM>("user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Token);

            return await _http.SendAsync(request);
        }

        // TODO UriBuilder => query params (if the uri doesn't contain last slash, the url will be invalid) 
        public async Task<HttpResponseMessage> DeleteItem(int id, string uri)
        {
            if (uri.Last() != '/')
                uri += '/';

            using var request = new HttpRequestMessage(HttpMethod.Delete, uri + id);

            var token = await _localStorage.GetItem<UserTokenVM>("user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Token);

            return await _http.SendAsync(request);
        }

        public async Task<HttpResponseMessage> CreateItem<T>(T record, string uri)
        {
            var postJson = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, uri);

            var token = await _localStorage.GetItem<UserTokenVM>("user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Token);
            request.Content = postJson;
            return await _http.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateItem<T>(T record, string uri, string? token = null)
        {
            var patchJson = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Put, uri);

            if (token == null)
            {
                var userToken = await _localStorage.GetItem<UserTokenVM>("user");
                token = userToken?.Token;
            }
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = patchJson;
            return await _http.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri + categoryId);

            var token = await _localStorage.GetItem<UserTokenVM>("user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Token);

            return await _http.SendAsync(request);
        }
    }
}
