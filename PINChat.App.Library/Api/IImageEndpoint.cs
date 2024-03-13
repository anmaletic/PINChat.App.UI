using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public interface IImageEndpoint
{
    string GetUserImage(UserModel user);
    string GetGroupImage(string id);
}