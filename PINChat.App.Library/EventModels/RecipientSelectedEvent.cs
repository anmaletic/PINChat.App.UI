using PINChat.App.Library.Models;

namespace PINChat.App.Library.EventModels;

public class RecipientSelectedEvent
{
    public TargetModel Recipient { get; set; }
    
    public RecipientSelectedEvent(TargetModel recipient)
    {
        Recipient = recipient;
    }
}