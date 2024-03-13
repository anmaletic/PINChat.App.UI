using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public class ImageEndpoint : IImageEndpoint
{
    public string GetUserImage(UserModel user)
    {
        var api = "https://pinchat-api.anmal.dev";
        
        return $"{api}/api/Image/GetUserImage/{user.Id}?t={DateTime.Now.Ticks}";
    }

    public string GetGroupImage(string id)
    {
        var api = "https://pinchat-api.anmal.dev";
        
        return $"{api}/api/Image/GetGroupImage/{id}?t={DateTime.Now.Ticks}";
    }
}