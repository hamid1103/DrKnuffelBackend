
namespace DrKnuffelBackEnd.Repositories.UserData;

public interface IExtraUserData
{
    Task AddAsync(Models.UserData data);
    Task<Models.UserData> GetAsync(Guid id);
    Task<Models.UserData> GetAsyncByUserId(string UID);
}