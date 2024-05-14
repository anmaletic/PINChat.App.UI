using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PINChat.App.Blazor.Authentication;
using PINChat.App.Library.Api;
using PINChat.App.Library.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PINChat.App.Blazor.Components;

public partial class UserDetails
{
    [Parameter] public required UserModel Recipient { get; set; }
    [Parameter] public required bool IsReadOnly { get; set; }
    [Parameter] public EventCallback OnClosed { get; set; }
    [Parameter] public EventCallback OnAccept { get; set; }
    
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    [Inject] public required ILoggedInUserModel LoggedInUser { get; set; }
    [Inject] public required IUserEndpoint UserEndpoint { get; set; }
    [Inject] public required IAuthenticationService AuthService { get; set; }
    
    public required UserModel TempUser { get; set; }
    
    private IBrowserFile? _selectedFile;
    private bool IsMessageVisible { get; set; }
    private bool IsLoading { get; set; }
    private string? DialogMessage { get; set; } = "";


    protected override Task OnInitializedAsync()
    {
        TempUser = new UserModel
        {
            Id = Recipient.Id,
            DisplayName = Recipient.DisplayName,
            FirstName = Recipient.FirstName,
            LastName = Recipient.LastName,
            Avatar = Recipient.Avatar,
            AvatarPath = Recipient.AvatarPath
        };
        
        return base.OnInitializedAsync();
    }

    private async Task OpenFileDialog(MouseEventArgs arg)
    {
        if(IsReadOnly) return;
        
        await JsRuntime.InvokeVoidAsync("blazorHelpers.triggerClick", "fileInput");
    }
    private async Task HandleFileChange(InputFileChangeEventArgs e)
    {
        try
        {
            _selectedFile = e.GetMultipleFiles().FirstOrDefault();
        
            switch (_selectedFile?.ContentType)
            {
                default:
                    DialogMessage = "Tip datoteke nije ispravan!";
                    IsMessageVisible = true;
                    break;
                case "image/jpeg":
                case "image/jpg":
                case "image/png":
                {
                    using var memoryStream = new MemoryStream();
                    await _selectedFile.OpenReadStream(50*1024).CopyToAsync(memoryStream);
                    var imgBytes = memoryStream.ToArray();
                    
                    TempUser.Avatar = imgBytes;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            DialogMessage = "Datoteka je veća od 50KB!";
            IsMessageVisible = true;
        }
    }

    private async Task UpdateUser()
    {        
        if (string.IsNullOrEmpty(TempUser.DisplayName) ||
            string.IsNullOrEmpty(TempUser.FirstName) ||
            string.IsNullOrEmpty(TempUser.LastName))
        {
            DialogMessage = "Nisu sva polja popunjena!";
            IsMessageVisible = true;
        }
        else
        {
            try
            {
                IsLoading = true;


                LoggedInUser.DisplayName = TempUser.DisplayName;
                LoggedInUser.FirstName = TempUser.FirstName;
                LoggedInUser.LastName = TempUser.LastName;
                LoggedInUser.Avatar = TempUser.Avatar;

                await UserEndpoint.UpdateUser(TempUser);

                await OnAccept.InvokeAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    private async Task LogoutUser()
    {
        await AuthService.Logout();
    }
    
    private void Close()
    {
        OnClosed.InvokeAsync();
    }

    private void OnMessageDialogClosed()
    {
        IsMessageVisible = false;
    }
    
    // todo: wire up image resizing
    public byte[] ResizeImage(byte[] imageData, int targetSizeInKb)
    {
        using var input = new MemoryStream(imageData);
        using var output = new MemoryStream();
        using var image = Image.Load(input);
        
        // Calculate target image size in bytes
        var targetSizeInBytes = targetSizeInKb * 1024;

        // Resize the image while maintaining aspect ratio
        var options = new ResizeOptions
        {
            Size = new Size(image.Width / 2, image.Height / 2), // Adjust dimensions as needed
            Mode = ResizeMode.Max
        };
        image.Mutate(x => x.Resize(options));

        // Compress the image to meet target size
        var encoder = new JpegEncoder
        {
            Quality = 80 // Adjust quality as needed
        };
        image.Save(output, encoder);

        // Ensure the resulting image size is below the target size
        if (output.Length <= targetSizeInBytes)
        {
            return output.ToArray();
        }
        else
        {
            // If the resulting image size is still too large, recursively adjust until it fits
            return ResizeImage(output.ToArray(), targetSizeInKb);
        }
    }
}