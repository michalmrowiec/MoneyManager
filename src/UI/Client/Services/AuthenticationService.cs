using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Pages.LoginUsers;
using MoneyManager.Client.ViewModels;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoneyManager.Client.Services
{
    internal interface IAuthenticationService
    {
        Task LogoutUser();
        Task<bool> LoginUserHelper(HttpResponseMessage loginResponseWithJwt);
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpRecordService _httpRecordService;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationState _authenticationState;

        public AuthenticationService(ILocalStorageService localStorageService, IHttpRecordService httpRecordService, NavigationManager navigationManager, AuthenticationState authenticationState)
        {
            _localStorage = localStorageService;
            _httpRecordService = httpRecordService;
            _navigationManager = navigationManager;
            _authenticationState = authenticationState;
        }

        public async Task LogoutUser()
        {
            await _localStorage.RemoveItem("user");
            _authenticationState.LoggedIn = false;
        }

        public async Task<bool> LoginUserHelper(HttpResponseMessage loginResponseWithJwt)
        {
            await LogoutUser();
            try
            {
                UserTokenVM userToken = JsonConvert.DeserializeObject<UserTokenVM>(await loginResponseWithJwt.Content.ReadAsStringAsync()) ?? new();
                var tokenHandler = new JwtSecurityTokenHandler();
                userToken.UserId = int.Parse(tokenHandler.ReadJwtToken(userToken.Token).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                await _localStorage.SetItem<UserTokenVM>("user", userToken);
                _authenticationState.LoggedIn = true;
                _navigationManager.NavigateTo("tracker");
                await _httpRecordService.GetListOfItems("api/recurring/ex");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
