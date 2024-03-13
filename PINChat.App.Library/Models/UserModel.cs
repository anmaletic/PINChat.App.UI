using System.Collections.ObjectModel;

namespace PINChat.App.Library.Models;

public class UserModel : TargetModel, ILoggedInUserModel
{
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? Avatar { get; set; }
    public override string? AvatarPath { get; set; }
    public ObservableCollection<UserModel> Contacts { get; set; } = [];
    public ObservableCollection<GroupModel> Groups { get; set; } = [];
    


    public void ResetUserModel()
    {
        Id = "";
        DisplayName = "";
        FirstName = "";
        LastName = "";
        Contacts = [];
        Groups = [];
    }
    
    public string FullName => $"{FirstName} {LastName}";
    public override string? Name => DisplayName;
    public override string? Type => "User";
}