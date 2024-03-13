using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public interface ISettingsEndpoint
{
    Task<SettingsModel> GetByKey(string key);
}