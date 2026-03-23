using Dapper;
using Microsoft.Data.SqlClient;

namespace DrKnuffelBackEnd.Repositories.Progress;

public class SQLProgressRepo : IProgressRepo
{
    
    private readonly string SqlString;

    public SQLProgressRepo(string sqlConnectionString)
    {
        SqlString = sqlConnectionString;
    }
    
    public async Task InsertAsync(Models.Progress data)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            await sqlConnection.ExecuteAsync("INSERT INTO [Progress] (Id, UserDataId, StepId, Completed, Completed_at) VALUES (@Id, @UserDataId, @StepId, @Completed, @Completed_at)", data);
        }
    }

    public async Task<IEnumerable<Models.Progress>> GetAsyncByUserId(string id)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QueryAsync<Models.Progress>("SELECT * FROM [Progress] WHERE UserDataId = @id", new { id });   
        };
    }
}