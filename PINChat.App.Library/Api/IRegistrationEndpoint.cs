namespace PINChat.App.Library.Api;

public interface IRegistrationEndpoint
{
    Task<string> Register(string username, string password);
}