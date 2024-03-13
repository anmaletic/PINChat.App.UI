using Microsoft.AspNetCore.Components;

namespace PINChat.App.Blazor.Components;

public partial class Login
{
    [Parameter] public EventCallback<bool> OnUserAuthenticated { get; set; }

    private bool IsRegistering { get; set; } = false;
    private bool IsLoading { get; set; } = false;

    private Task SetUserState(string state)
    {
        IsRegistering = state == "r";
        return Task.CompletedTask;
    }

    private void LogInCallback()
    {
        OnUserAuthenticated.InvokeAsync(true);
    }

    private void SetLoadingState(bool state)
    {
        IsLoading = state;
    }
}