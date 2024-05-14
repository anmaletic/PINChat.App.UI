﻿using System.Net.Http.Headers;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PINChat.App.Blazor.Models;
using PINChat.App.Library.Models;

namespace PINChat.App.Blazor.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly string _authTokenStorageKey;
    private readonly HttpClient _client;
    private readonly IConfiguration _config;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IConfiguration config)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _config = config;

        _authTokenStorageKey = _config["authTokenStorageKey"];
    }

    public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
    {
        var data = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", userForAuthentication.UserName!),
            new KeyValuePair<string, string>("password", userForAuthentication.Password!)
        });

        var api = _config["api"] + _config["tokenEndpoint"];

        var authResult = await _client.PostAsync(api, data);
        var authContent = await authResult.Content.ReadAsStringAsync();

        if (authResult.IsSuccessStatusCode == false) return null;
        var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
            authContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        await _localStorage.SetItemAsync(_authTokenStorageKey, result!.Access_Token);

        await ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token!);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

        return result;
    }

    public async Task Logout()
    {
        await ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}