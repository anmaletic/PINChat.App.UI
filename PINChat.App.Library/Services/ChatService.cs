using Microsoft.AspNetCore.SignalR.Client;

namespace PINChat.App.Library.Services;

public class ChatService : IChatService
{
    public HubConnection? HubConnection { get; set; } = null!;

    public async Task AddGroup(string groupId)
    {
        await HubConnection!.InvokeAsync("AddGroup", groupId);
    }
    
    public async Task RemoveGroup(string groupId)
    {
        await HubConnection!.InvokeAsync("RemoveGroup", groupId);
    }
}