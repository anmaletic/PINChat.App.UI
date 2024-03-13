using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PINChat.App.Library.Models;

public class TargetModel : IRecipientModel
{
    private string? _id;

    public string? Id
    {
        get => _id;
        set
        {
            _id = value;
            NotifyPropertyChanged();
        }
    }
    
    public virtual string? AvatarPath { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Type { get; set; }

    public void Reset()
    {
        Id = "";
        AvatarPath = "";
        Name = "";
    }
    
    public event EventHandler? PropertyChanged;
    protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}