using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public interface IUserEndpoint
{
    Task<AuthenticatedUserModel> Authenticate(string username, string password);
    Task GetLoggedInUserInfo(string token);
    Task<List<UserModel>> GetAll();
    Task<string> AddContact(string userId, string contactId);
    Task<string> RemoveContact(string userId, string contactId);
    Task<string> AddGroup(string userId, string groupId);
    Task<string> RemoveGroup(string userId, string groupId);
}