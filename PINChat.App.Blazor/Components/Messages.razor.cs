using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PINChat.App.Library.Api;
using PINChat.App.Library.Models;
using PINChat.App.Library.Services;

namespace PINChat.App.Blazor.Components;

public partial class Messages
{
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    [Inject] public required ILoggedInUserModel LoggedInUser { get; set; }
    [Inject] public required IUserEndpoint UserEndpoint { get; set; }
    [Inject] public required IMessageEndpoint MessageEndpoint { get; set; }
    [Inject] public required IChatService ChatService { get; set; }
    
    private List<MessageModel> MessageList { get; set; } = [];
    public required TargetModel SelectedRecipient { get; set; }    
    private UserModel UserTarget { get; set; } = new();
    private string MessageContent { get; set; } = "";
    private bool PreventDefault { get; set; } = false;
    private bool IsModalVisible { get; set; }
    private bool IsReadOnly { get; set; }


    protected override void OnInitialized()
    {
        ChatService.MessageReceived += OnMessageReceived;
    }

    private async void OnMessageReceived(string action, MessageDtoModel message)
    {
        var msg = new MessageModel
        {
            CreatedDate = message.CreatedDate,
            TargetId = message.TargetId,
            SourceId = message.SourceId,
            AvatarPath = message.SourceAvatar,
            SourceName = message.SourceName,
            Content = message.Content,
            Image = message.Image,
            IsOrigin = LoggedInUser.Id == message.SourceId
        };
        
        if ((action == "AddSingle" && msg.SourceId == SelectedRecipient.Id) ||
            (action == "AddGroup" && msg.TargetId == SelectedRecipient.Id))
        {
            MessageList.Add(msg);

            await ScrollToBottomAuto();
            StateHasChanged();
        }
    }
    private async Task OnRecipientSelected(TargetModel target)
    {
        SelectedRecipient = target;
        MessageList.Clear();
        
        MessageQueryModel msgModel = new()
        {
            SourceId = LoggedInUser.Id,
            TargetId = SelectedRecipient.Id
        };
        
        MessageList = SelectedRecipient switch
        {
            UserModel => await MessageEndpoint.GetByUserId(msgModel),
            GroupModel => await MessageEndpoint.GetByGroupId(msgModel),
            _ => MessageList
        };
        
        foreach (var message in MessageList)
        {
            message.IsOrigin = LoggedInUser.Id == message.SourceId;
        }
        await ScrollToBottomAuto();        
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        try
        {
            if (e is { Key: "Enter", ShiftKey: false })
            {
                PreventDefault = true;
                await Task.Yield();
                await SendMessage();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            PreventDefault = false;
        }

    }    
    private async Task OnInputBoxChange(ChangeEventArgs e)
    {
        MessageContent = e.Value?.ToString() ?? "";

        await UpdateTexboxHeight();
        await ScrollToBottomManual();
    }
    private async Task SendMessage()
    {
        if(string.IsNullOrWhiteSpace(MessageContent)) return;

        try
        {
            var dtoMessage = new MessageDtoModel()
            {
                CreatedDate = DateTime.UtcNow,
                TargetId = SelectedRecipient.Id,
                SourceId = LoggedInUser.Id,
                SourceName = LoggedInUser.FullName,
                SourceAvatar = LoggedInUser.AvatarPath,
                Content = MessageContent,
                Image = ""
            };

            switch (SelectedRecipient)
            {
                case UserModel:
                    await ChatService.SendSingleMessage(dtoMessage);
                    break;
                case GroupModel:
                    await ChatService.SendGroupMessage(dtoMessage);
                    break;
            }
            
            // save to database
            await MessageEndpoint.CreateNew(dtoMessage);
            
            var displayMessage = new MessageModel()
            {
                CreatedDate = DateTime.UtcNow,
                TargetId = SelectedRecipient.Id,
                SourceId = LoggedInUser.Id,
                Content = MessageContent,
                Image = "",
                IsOrigin = true
            };
            MessageList.Add(displayMessage);
            
            MessageContent = "";

            await ScrollToBottomAuto();
            await ResetTexboxHeight();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task ShowUserDetails(string id)
    {
        UserTarget = (await UserEndpoint.GetAll()).FirstOrDefault(x=>x.Id == id)!;
        IsReadOnly = true;
        IsModalVisible = true;
    }
    private void OnProfileClosed()
    {
        IsModalVisible = false;
    }

    private async Task UpdateTexboxHeight()
    {
        await JsRuntime.InvokeVoidAsync("updateTextBoxHeight", "inputBox");
    }
    private async Task ResetTexboxHeight()
    {
        await JsRuntime.InvokeVoidAsync("resetTextBoxHeight", "inputBox");
    }
    private async Task ScrollToBottomAuto()
    {
        await JsRuntime.InvokeVoidAsync("scrollToBottomAuto", "messages-output");
    }
    private async Task ScrollToBottomManual()
    {
        await JsRuntime.InvokeVoidAsync("scrollToBottomManual", "messages-output");
    }

}