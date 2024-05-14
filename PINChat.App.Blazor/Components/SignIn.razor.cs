using Microsoft.AspNetCore.Components;
using PINChat.App.Blazor.Authentication;
using PINChat.App.Blazor.Models;
using PINChat.App.Library.Api;

namespace PINChat.App.Blazor.Components;

public partial class SignIn
{
    [Parameter] public EventCallback OnLogin { get; set; }
    [Parameter] public EventCallback<bool> OnLoading { get; set; }
    
    [Inject] private IUserEndpoint UserEndpoint { get; set; } = null!;
    [Inject] private IAuthenticationService AuthService { get; set; } = null!;

    private AuthenticationUserModel _authModel = new();

    private string Message { get; set; } = "";
    private bool IsMessageVisible { get; set; }
    
    private async Task LoginUser()
    {
        try
        {
            Message = "";
            IsMessageVisible = false;
            await OnLoading.InvokeAsync(true);
            
            await AuthService.Login(_authModel);
            
            await OnLogin.InvokeAsync();
        }
        catch (Exception e)
        {
            Message = "Pogrešno korisničko ime ili lozinka!";
            IsMessageVisible = true;
            Console.WriteLine(e);
        }
        finally
        {
            await OnLoading.InvokeAsync(false);
        }
    }
}