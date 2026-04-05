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
            await sqlConnection.ExecuteAsync("INSERT INTO [Progress] (Id, UserData_id, Step_id, Completed, Completed_at) VALUES (@Id, @UserData_id, @Step_id, @Completed, @Completed_at)", data);
        }
    }

    public async Task<IEnumerable<Models.Progress>> GetAsyncByUserDataId(string id)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QueryAsync<Models.Progress>("SELECT * FROM [Progress] WHERE UserData_id = @id", new { id });   
        };
    }
}