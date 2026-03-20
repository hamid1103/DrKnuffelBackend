namespace DrKnuffelBackEnd.Repositories.Progress;

public interface IProgressRepo
{
    Task InsertAsync(Models.Progress progress);
    Task<IEnumerable<Models.Progress>> SelectAsync();
    Task<IEnumerable<Models.Progress>> GetAsyncByUserId(Guid id);
    Task<IEnumerable<Models.Progress>> GetAsyncById(Guid id);
}