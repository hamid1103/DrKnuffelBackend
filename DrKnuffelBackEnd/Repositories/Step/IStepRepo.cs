namespace DrKnuffelBackEnd.Repositories.Step;

public interface IStepRepo
{
    Task InsertAsync(Models.Step data);
    Task<Models.Step> GetStepById(Guid id);
    Task<IEnumerable<DrKnuffelBackEnd.Models.Step>> GetSteps();
    Task<Models.Step> GetStepByTitle(string name);
    Task<Models.Step> GetStepByStepOrder(int index);
}