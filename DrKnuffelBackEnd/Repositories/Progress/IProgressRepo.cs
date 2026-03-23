namespace DrKnuffelBackEnd.Repositories.Progress;

public interface IProgressRepo
{
    Task InsertAsync(Models.Progress data);
    Task<IEnumerable<Models.Progress>> GetAsyncByUserId(string id);
}