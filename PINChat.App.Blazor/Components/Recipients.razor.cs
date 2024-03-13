using Microsoft.AspNetCore.Components;
using PINChat.App.Library.Models;

namespace PINChat.App.Blazor.Components;

public partial class Recipients
{
    [Parameter] public EventCallback<TargetModel> OnRecipientSelected { get; set; }
    [Parameter] public required string Caller { get; set; }
    
    [Inject] public ILoggedInUserModel LoggedInUser { get; set; } = null!;
    [Inject] public IRecipientModel Recipient { get; set; } = null!;

    private bool IsUpdatingRecipient { get; set; }

    private async void SelectRecipient(TargetModel target)
    {
        if(IsUpdatingRecipient) return;

        IsUpdatingRecipient = true;
        try
        {
            Recipient.Id = target.Id;
            Recipient.AvatarPath = target.AvatarPath;
            Recipient.Name = target.Name;
            Recipient.Type = target.Type;
            
            await OnRecipientSelected.InvokeAsync(target);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await Task.Delay(100);
        IsUpdatingRecipient = false;
    }
    private string ContactIsSelected(string contactId)
    {
        return Recipient.Id == contactId ? "selected" : "";
    }
}