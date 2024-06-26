﻿using System.Collections.ObjectModel;

namespace PINChat.App.Library.Models;

public interface ILoggedInUserModel
{
    string? Id { get; set; }
    string? DisplayName { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    byte[]? Avatar { get; set; }
    string? AvatarPath { get; set; }
    public ObservableCollection<UserModel> Contacts { get; set; }
    public ObservableCollection<GroupModel> Groups { get; set; }
    string FullName { get; }
    void ResetUserModel();
}