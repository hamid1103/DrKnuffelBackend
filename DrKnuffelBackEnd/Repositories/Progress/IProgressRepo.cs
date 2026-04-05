namespace DrKnuffelBackEnd.Repositories.Progress;

public interface IProgressRepo
{
    Task InsertAsync(Models.Progress data);
    Task<IEnumerable<Models.Progress>> GetAsyncByUserDataId(string id);
}