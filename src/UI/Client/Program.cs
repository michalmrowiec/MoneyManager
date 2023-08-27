using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoneyManager.Client;
using MoneyManager.Client.Components.AddEditRecord;
using MoneyManager.Client.Pages.LoginUsers;
using MoneyManager.Client.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IDisplayService, DisplayService>(provider => new DisplayService(new CultureInfo("en-US")));
builder.Services.AddSingleton<AuthenticationState>();
builder.Services.AddSingleton<RecordFormDialogEventHelper>();
builder.Services.AddSingleton<ErrorMessage>();
builder.Services.AddSingleton<NameOfSubpage>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    httpClient.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiKey"]);
    return httpClient;
});


builder.Services.AddScoped<ILocalStorageService, LocalStorage>();
builder.Services.AddScoped<IHttpRecordService, HttpRecordService>();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

await builder.Build().RunAsync();
