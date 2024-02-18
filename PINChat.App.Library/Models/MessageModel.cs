namespace PINChat.App.Library.Models;

public class MessageModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? TargetId { get; set; }
    public string? SourceId { get; set; }
    public string? SourceName { get; set; }
    public string? SourceAvatar { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public bool IsOrigin { get; set; }
}