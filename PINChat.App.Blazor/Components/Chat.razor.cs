using Microsoft.AspNetCore.Components;
using PINChat.App.Library.Api;
using PINChat.App.Library.Models;
using PINChat.App.Library.Services;

namespace PINChat.App.Blazor.Components;

public partial class Chat
{
    [Inject] private IUserEndpoint UserEndpoint { get; set; } = null!;
    [Inject] private IImageEndpoint ImageEndpoint { get; set; } = null!;
    [Inject] private ILoggedInUserModel LoggedInUser { get; set; } = null!;
    [Inject] private IRecipientModel Recipient { get; set; } = null!;
    [Inject] private IChatService ChatService { get; set; } = null!;

    private UserModel UserTarget { get; set; } = new();
    private string SelectedHeaderItem { get; set; } = "Messages";
    private bool IsModalVisible { get; set; }
    private bool IsReadOnly { get; set; }
    
    
    protected override async Task OnInitializedAsync()
    {
        if (LoggedInUser.AvatarPath == null)
        {

            LoggedInUser.AvatarPath = $"https://pinchat-api.anmal.dev/api/Image/GetUserImage/{LoggedInUser.Id}";
            await UserEndpoint.UpdateUser((LoggedInUser as UserModel)!);
            
            OnUpdate();
        }
        
        Recipient.PropertyChanged += async (sender, args) => await InvokeAsync(StateHasChanged);
        await SetupChatHub();
    }
    private async Task SetupChatHub()
    {
        if (!ChatService.IsConnected && !string.IsNullOrEmpty(LoggedInUser.Id))
        {
            await ChatService.Connect();
            await ChatService.Login(LoggedInUser.Id);
            
            List<string> groups = LoggedInUser.Groups.Select(x => x.Id).ToList()!;
            foreach (var group in groups)
            {
                await ChatService.AddGroup(group);
            }
        }
    }
    
    private void SelectHeaderItem(string selection)
    {
        Recipient.Reset();
        
        if (SelectedHeaderItem == selection)
        {
            SelectedHeaderItem = "Messages";
            return;
        }
        SelectedHeaderItem = selection;
    }

    private async Task ShowDetails(string target)
    {
        switch (target)
        {
            case "User":
                UserTarget = (LoggedInUser as UserModel)!;
                IsReadOnly = false;
                IsModalVisible = true;
                break;
            case "Recipient":
                if (Recipient.Type == "User")
                {
                    // replace with UserEndpoing.GetById(Recipient.Id)
                    UserTarget = (await UserEndpoint.GetAll()).FirstOrDefault(x=>x.Id == Recipient.Id)!;
                    IsReadOnly = true;
                    IsModalVisible = true;
                }
                break;
        }
    }
    private void OnProfileClosed()
    {
        IsModalVisible = false;
    }
    private void OnUpdate()
    {
        LoggedInUser.AvatarPath = ImageEndpoint.GetUserImage((LoggedInUser as UserModel)!);
    }

}