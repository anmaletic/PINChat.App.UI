using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using PINChat.App.Library.Models;

namespace PINChat.App.Library.Services;

public class ChatService : IChatService
{
    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
    public event Action<string, MessageDtoModel>? MessageReceived;
    
    private readonly HubConnection _hubConnection;


    public ChatService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://pinchat-server.anmal.dev/chathub", options =>
            {
                options.AccessTokenProvider = async () => await Task.FromResult("YourAccessToken");
                options.Transports = HttpTransportType.WebSockets;
            })
            .WithAutomaticReconnect([TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)])
            .Build();
        
        _hubConnection.On<string, MessageDtoModel>("ReceiveMessage", (action, message) => MessageReceived?.Invoke(action, message));
    }
    
    public async Task<bool> Connect()
    {
        try
        {
            await _hubConnection.StartAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> Login(string userName)
    {
        try
        {
            await _hubConnection.InvokeAsync("Login", userName);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public async Task AddGroup(string groupId)
    {
        await _hubConnection.InvokeAsync("AddGroup", groupId);
    }
    public async Task RemoveGroup(string groupId)
    {
        await _hubConnection.InvokeAsync("RemoveGroup", groupId);
    }
    
    public async Task SendSingleMessage(MessageDtoModel dtoMessage)
    {
        await _hubConnection.SendAsync("MessageSingle", "AddSingle", dtoMessage);
    }
    public async Task SendGroupMessage(MessageDtoModel dtoMessage)
    {
        await _hubConnection.SendAsync("MessageGroup", "AddGroup", dtoMessage);
    }
}