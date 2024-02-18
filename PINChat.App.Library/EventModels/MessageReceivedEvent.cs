using PINChat.App.Library.Models;

namespace PINChat.App.Library.EventModels;

public class MessageReceivedEvent
{
    public string Action { get; set; }
    public MessageDbModel Message { get; set; }
    
    public MessageReceivedEvent(string action, MessageDbModel message)
    {
        Message = message;
        Action = action;
    }
}