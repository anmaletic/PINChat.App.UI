using PINChat.App.Library.Models;

namespace PINChat.App.Library.EventModels;

public class ChangeUserDataEvent
{
    public TargetModel Target { get; set; }
    
    public ChangeUserDataEvent(TargetModel target)
    {
        Target = target;
    }
}