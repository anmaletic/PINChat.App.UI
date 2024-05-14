using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PINChat.App.Blazor;
using PINChat.App.Blazor.Authentication;
using PINChat.App.Library.Api;
using PINChat.App.Library.Models;
using PINChat.App.Library.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddSingleton<IApiHelper, ApiHelper>();
builder.Services.AddSingleton<ILoggedInUserModel, UserModel>();
builder.Services.AddSingleton<IRecipientModel, TargetModel>();

builder.Services.AddScoped<IUserEndpoint, UserEndpoint>();
builder.Services.AddScoped<IGroupEndpoint, GroupEndpoint>();

builder.Services.AddSingleton<IChatService, ChatService>();

builder.Services.AddScoped<IRegistrationEndpoint, RegistrationEndpoint>();

builder.Services.AddScoped<IImageEndpoint, ImageEndpoint>();
builder.Services.AddScoped<IMessageEndpoint, MessageEndpoint>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();