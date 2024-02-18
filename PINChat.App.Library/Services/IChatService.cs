using Microsoft.AspNetCore.SignalR.Client;

namespace PINChat.App.Library.Services;

public interface IChatService
{
    HubConnection? HubConnection { get; set; }
    Task AddGroup(string groupId);
    Task RemoveGroup(string groupId);
}