namespace PINChat.App.Library.EventModels;

public class LoggedInEvent
{
    public bool State { get; set; }

    public LoggedInEvent(bool state)
    {
        State = state;
    } 
}