namespace DrKnuffelBackEnd.Repositories.UserRole;

public interface IUserRoleRepo
{
    Task InserAsync(Models.UserRole data);
    Task<Models.UserRole> GetByIdAsync(Guid id);
    Task<Models.UserRole> GetByRoleName(string name);
    Task<IEnumerable<Models.UserRole>> GetAsync();
}