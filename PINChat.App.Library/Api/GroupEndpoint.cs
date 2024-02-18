using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public class GroupEndpoint : IGroupEndpoint
{    
    private readonly IApiHelper _apiHelper;

    public GroupEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    public async Task<List<GroupModel>> GetAll()
    {
        using var response = await _apiHelper.ApiClient.GetAsync("api/Group/GetAll");

        if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        
        var result = await response.Content.ReadAsAsync<List<GroupModel>>();
        return result;
    }
}