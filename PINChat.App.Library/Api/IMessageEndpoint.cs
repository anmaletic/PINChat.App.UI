using PINChat.App.Library.Models;

namespace PINChat.App.Library.Api;

public interface IMessageEndpoint
{
    Task<List<MessageModel>> GetByUserId(MessageQueryModel msg);
    Task<List<MessageModel>> GetByGroupId(MessageQueryModel msg);
    Task<string> CreateNew(MessageDbModel model);
}