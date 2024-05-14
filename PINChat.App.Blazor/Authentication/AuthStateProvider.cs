using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PINChat.App.Library.Api;

namespace PINChat.App.Blazor.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymous;
    private readonly IApiHelper _apiHelper;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly IUserEndpoint _userEndpoint;

    public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration config,
        IApiHelper apiHelper, IUserEndpoint userEndpoint)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _config = config;
        _apiHelper = apiHelper;
        _userEndpoint = userEndpoint;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authTokenStorageKey = _config["authTokenStorageKey"];
        var token = await _localStorage.GetItemAsync<string>(authTokenStorageKey);

        if (string.IsNullOrWhiteSpace(token)) return _anonymous;

        var isAuthenticated = await NotifyUserAuthentication(token);

        if (isAuthenticated == false) return _anonymous;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
                    "jwtAuthType")));
    }

    public async Task<bool> NotifyUserAuthentication(string token)
    {
        bool isAuthenticatedOutput;
        Task<AuthenticationState> authState;

        try
        {
            await _userEndpoint.GetLoggedInUserInfo(token);
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
                    "jwtAuthType"));
            authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
            isAuthenticatedOutput = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await NotifyUserLogout();
            isAuthenticatedOutput = false;
        }

        return isAuthenticatedOutput;
    }

    public async Task NotifyUserLogout()
    {
        var authTokenStorageKey = _config["authTokenStorageKey"];
        await _localStorage.RemoveItemAsync(authTokenStorageKey);

        var authState = Task.FromResult(_anonymous);
        _userEndpoint.LogOffUser();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(authState);
    }
}