using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public interface IGroupEndpoint
{
    Task<List<GroupModel>> GetAll();
}