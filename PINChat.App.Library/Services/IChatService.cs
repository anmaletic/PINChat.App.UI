using Microsoft.AspNetCore.SignalR.Client;
using PINChat.App.Library.Models;

namespace PINChat.App.Library.Services;

public interface IChatService
{
    Task AddGroup(string groupId);
    Task RemoveGroup(string groupId);
    Task SendSingleMessage(MessageDtoModel dtoMessage);
    Task SendGroupMessage(MessageDtoModel dtoMessage);
    Task<bool> Connect();
    Task<bool> Login(string userName);
    bool IsConnected { get; }
    event Action<string, MessageDtoModel>? MessageReceived;
}