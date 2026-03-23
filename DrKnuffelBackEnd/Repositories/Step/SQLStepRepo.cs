using Dapper;
using Microsoft.Data.SqlClient;

namespace DrKnuffelBackEnd.Repositories.Step;

public class SQLStepRepo : IStepRepo
{
    private readonly string SqlString;

    public SQLStepRepo(string sqlConnectionString)
    {
        SqlString = sqlConnectionString;
    }
    public async Task InsertAsync(Models.Step data)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            await sqlConnection.ExecuteAsync("INSERT INTO [Step] (Id, Title, Description, Step_order) VALUES (@Id, @Title, @Description, @Step_order)", data);
        }
    }

    public async Task<Models.Step> GetStepById(Guid id)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QuerySingleAsync<Models.Step>("SELECT * FROM [Step] WHERE Id = @id", new { id });   
        };
    }

    public async Task<IEnumerable<Models.Step>> GetSteps()
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QueryAsync<Models.Step>("SELECT * FROM [Step]");   
        };
    }

    public async Task<Models.Step> GetStepByTitle(string name)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QuerySingleAsync<Models.Step>("SELECT * FROM [Step] WHERE Title = @id", new { name });   
        };
    }
}