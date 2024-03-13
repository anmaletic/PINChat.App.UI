using Microsoft.AspNetCore.Components;
using PINChat.App.Library.Api;
using PINChat.App.Library.Models;

namespace PINChat.App.Blazor.Components;

public partial class Settings
{
    [Parameter] public required string Caller { get; set; }
    [Inject] public required ILoggedInUserModel LoggedInUser { get; set; }
    [Inject] public required IUserEndpoint UserEndpoint { get; set; }
    [Inject] public required IGroupEndpoint GroupEndpoint { get; set; }
    
    private List<UserModel> _allContacts = [];
    private List<GroupModel> _allGroups = [];
    private List<TargetModel> RecipientList { get; set; } = [];
    
    private bool IsLoading { get; set; }
    private bool IsUpdatingRecipient { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            await Task.Run(async () =>
            {
                switch (Caller)
                {
                    case "Contacts":
                        await LoadContacts();
                        break;
                    case "Groups":
                        await LoadGroups();
                        break;
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            IsLoading = false;
        }

    }
    private async Task LoadContacts()
    {
        _allContacts = await UserEndpoint.GetAll();
        _allContacts = _allContacts.ExceptBy(LoggedInUser.Contacts.Select(s => s.Id), contact => contact.Id).ToList();
        _allContacts.Remove(_allContacts.FirstOrDefault(x => x.Id == LoggedInUser.Id!)!);
        
        RecipientList = [.._allContacts.OrderBy(x => x.FullName).ToList()];
    }
    private async Task LoadGroups()
    {
        _allGroups = await GroupEndpoint.GetAll();
        _allGroups = _allGroups.ExceptBy(LoggedInUser.Groups.Select(s => s.Id), group => group.Id).ToList();
        
        RecipientList = [.._allGroups.OrderBy(x => x.Name).ToList()];
    }
    
    private async Task AddRecipient(TargetModel recipient)
    { 
        if(IsUpdatingRecipient) return;
        
        IsUpdatingRecipient = true;
        switch (Caller)
        {
            case "Contacts":
                await AddContact(recipient);
                break;
            case "Groups":
                await AddGroup(recipient);
                break;
        }

        await Task.Delay(100);
        IsUpdatingRecipient = false;
    }
    private async Task AddContact(TargetModel recipient)
    {
        try
        {
            var exists = LoggedInUser.Contacts.Any(x => x.Id == recipient.Id);
            if (exists) return;
            
            await UserEndpoint.AddContact(LoggedInUser.Id!, recipient.Id!);
            
            var user = (recipient as UserModel)!;
            LoggedInUser.Contacts.Add(user);
            _allContacts.Remove(user);
            RecipientList.Remove(user);
            
            LoggedInUser.Contacts = [..LoggedInUser.Contacts.OrderBy(x => x.FullName).ToList()];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private async Task AddGroup(TargetModel recipient)
    {
        try
        {
            var exists = LoggedInUser.Groups.Any(x => x.Id == recipient.Id);
            if (exists) return;
            
            await UserEndpoint.AddGroup(LoggedInUser.Id!, recipient.Id!);
            
            var group = (recipient as GroupModel)!;
            LoggedInUser.Groups.Add(group);
            _allGroups.Remove(group);
            RecipientList.Remove(group);
            
            LoggedInUser.Groups = [..LoggedInUser.Groups.OrderBy(x => x.Name).ToList()];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task RemoveRecipient(TargetModel recipient)
    {
        switch (Caller)
        {
            case "Contacts":
                await RemoveContact(recipient);
                break;
                
            case "Groups":
                await RemoveGroup(recipient);
                break;
        }
    }
    private async Task RemoveContact(TargetModel recipient)
    {
        var exists = _allContacts.Any(x => x.Id == recipient.Id);
        if (exists) return;
        
        await UserEndpoint.RemoveContact(LoggedInUser.Id!, recipient.Id!);
                
        var user = (recipient as UserModel)!;
        LoggedInUser.Contacts.Remove(user);
        _allContacts.Add(user);
        RecipientList.Add(user);
        RecipientList = [..RecipientList.OrderBy(x => x.Name).ToList()];
    }
    private async Task RemoveGroup(TargetModel recipient)
    {
        var exists = _allGroups.Any(x => x.Id == recipient.Id);
        if (exists) return;

        await UserEndpoint.RemoveGroup(LoggedInUser.Id!, recipient.Id!);
        
        var group = (recipient as GroupModel)!;
        LoggedInUser.Groups.Remove(group);
        _allGroups.Add(group);
        RecipientList.Add(group);
        RecipientList = [..RecipientList.OrderBy(x => x.Name).ToList()];
    }

    private void OnFilterValueChange(ChangeEventArgs e)
    {
        var filter = e.Value!.ToString();
        
        switch (Caller)
        {
            case "Contacts":
                if (string.IsNullOrEmpty(filter))
                {
                    RecipientList = [.._allContacts.OrderBy(x => x.FullName).ToList()];
                }
                else
                {
                    RecipientList = [.._allContacts.Where(x => x.FullName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))];
                }
                break;
            case "Groups":
                if (string.IsNullOrEmpty(filter))
                {
                    RecipientList = [.._allGroups.OrderBy(x => x.Name).ToList()];
                }
                else
                {
                    RecipientList = [.._allGroups.Where(x => x.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase))];
                }                
                break;
        }
    }
}