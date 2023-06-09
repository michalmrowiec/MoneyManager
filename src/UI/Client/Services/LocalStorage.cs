using MoneyManager.Client.ViewModels;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public interface ILocalStorageService
    {
        Task<UserTokenVM> GetItem<T>(string key);
        Task<T?> GetItem2<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorage : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorage(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<UserTokenVM> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return new UserTokenVM();

            var result = JsonSerializer.Deserialize<UserTokenVM>(json);

            if (result == null)
                return new UserTokenVM();

            return result;
        }

        public async Task<T?> GetItem2<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            var r = JsonSerializer.Deserialize<T>(json);
            return r;
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}