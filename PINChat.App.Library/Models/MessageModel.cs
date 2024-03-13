namespace PINChat.App.Library.Models;

public class MessageModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? TargetId { get; set; }
    public string? SourceId { get; set; }
    public string? SourceName { get; set; }
    public string? AvatarPath { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public bool IsOrigin { get; set; }
    
    // todo: create MessageModel in blazor project with Css attribute
    // todo: remove Css attribute
    public string Css => IsOrigin ? "right-aligned" : "left-aligned";
}