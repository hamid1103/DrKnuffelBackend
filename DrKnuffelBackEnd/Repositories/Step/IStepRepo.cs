namespace DrKnuffelBackEnd.Repositories.Step;

public interface IStepRepo
{
    Task InsertAsync(Models.Step data);
    Task<Models.Step> GetStepById(Guid id);
    Task<Models.Step> GetStepByTitle(string name);
}