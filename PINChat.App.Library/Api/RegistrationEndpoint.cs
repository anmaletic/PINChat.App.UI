using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public class RegistrationEndpoint : IRegistrationEndpoint
{
    private readonly IApiHelper _apiHelper;

    public RegistrationEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    public async Task<string> Register(string username, string password)
    {
        var user = new { username, password };
            
        using var response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Registration", user);
        
        var msg = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            return msg;
        }

        throw new Exception(msg);
    }
}