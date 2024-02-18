namespace PINChat.App.Library.EventModels;

public class SetActiveChatViewMessage
{
    public string? ActiveModule { get; set; }

    public SetActiveChatViewMessage(string module)
    {
        ActiveModule = module;
    }
}