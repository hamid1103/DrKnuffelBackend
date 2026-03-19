using Dapper;
using Microsoft.Data.SqlClient;

namespace DrKnuffelBackEnd.Repositories.UserRole;

public class SQLUserRoleRepo : IUserRoleRepo
{
    private readonly string SQLString;

    public SQLUserRoleRepo(string sqlString)
    {
        SQLString = sqlString;
    }


    public async Task InserAsync(Models.UserRole data)
    {
        using (var sqlConnection = new SqlConnection(SQLString))
        {
            await sqlConnection.ExecuteAsync("INSERT INTO [UserRole] (Id, Name) VALUES (@Id, @Name)", data);
        }
    }

    public async Task<Models.UserRole> GetByIdAsync(Guid id)
    {
        using (var sqlConnection = new SqlConnection(SQLString))
        {
            return await sqlConnection.QuerySingleAsync<Models.UserRole>("SELECT * FROM [Progress] WHERE Id = @id", new { id });   
        };
    }

    public async Task<Models.UserRole> GetByRoleName(string name)
    {
        using (var sqlConnection = new SqlConnection(SQLString))
        {
            return await sqlConnection.QuerySingleAsync<Models.UserRole>("SELECT * FROM [Progress] WHERE Name = @name", new { name });   
        };
    }

    public async Task<IEnumerable<Models.UserRole>> GetAsync()
    {
        using (var sqlConnection = new SqlConnection(SQLString))
        {
            return await sqlConnection.QueryAsync<Models.UserRole>("SELECT * FROM [Progress]");   
        };
    }
}