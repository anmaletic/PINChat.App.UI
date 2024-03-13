using Microsoft.AspNetCore.Components;
using PINChat.App.Blazor.Models;
using PINChat.App.Library.Api;

namespace PINChat.App.Blazor.Components;

public partial class SignUp
{    
    [Parameter] public EventCallback<bool> OnLoading { get; set; }

    [Inject] private IRegistrationEndpoint RegistrationEndpoint { get; set; } = null!;
    
    private RegistrationModel _registrationModel = new();
    
    private string Message { get; set; } = "";
    private bool IsMessageVisible { get; set; }
    
    private async Task RegisterUser()
    {
        try
        {
            Message = "";
            IsMessageVisible = false;
            await OnLoading.InvokeAsync(true);

            await RegistrationEndpoint.Register(_registrationModel.UserName!, _registrationModel.Password!);
            
            Message = "Uspješno ste se registrirali!";
            IsMessageVisible = true;
            
            _registrationModel = new RegistrationModel();
        }
        catch (Exception e)
        {
            Message = "Došlo je do pogreške!";
            IsMessageVisible = true;
            Console.WriteLine(e);
        }
        finally
        {
            await OnLoading.InvokeAsync(false);
        }
    }
}