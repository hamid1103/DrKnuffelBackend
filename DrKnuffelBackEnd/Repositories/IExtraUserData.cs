using DrKnuffelBackEnd.Models;

namespace DrKnuffelBackEnd.Repositories;

public interface IExtraUserData
{
    Task AddAsync(UserData data);
    Task<UserData> GetAsync(Guid id);
}