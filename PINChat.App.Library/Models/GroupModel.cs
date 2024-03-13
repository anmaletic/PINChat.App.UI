namespace PINChat.App.Library.Models;

public class GroupModel : TargetModel
{
    public override string? Name { get; set; }
    public List<UserModel> Contacts { get; set; } = new ();
    public byte[]? Avatar { get; set; }  
    public override string? AvatarPath { get; set; }
    public override string? Type => "Group";

}