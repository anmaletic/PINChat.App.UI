using PINChat.App.Blazor.Models;
using PINChat.App.Library.Models;

namespace PINChat.App.Blazor.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication);
    Task Logout();
}