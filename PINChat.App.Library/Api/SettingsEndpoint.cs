using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public class SettingsEndpoint : ISettingsEndpoint
{
    private readonly IApiHelper _apiHelper;

    public SettingsEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    
    public async Task<SettingsModel> GetByKey(string key)
    {
        using var response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Setting/GetByKey", new SettingsModel() { Key = key});

        if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

        var result = await response.Content.ReadAsAsync<SettingsModel>();
        return result;
    }
}