using System.Runtime.CompilerServices;

namespace PINChat.App.Library.Models;

public interface IRecipientModel
{
    string? Id { get; set; }
    string? AvatarPath { get; set; }
    string? Name { get; set; }
    string? Type { get; set; }
    
    void Reset();
    
    event EventHandler? PropertyChanged;
}