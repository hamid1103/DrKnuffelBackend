using System.Data.SqlTypes;
using Dapper;
using DrKnuffelBackEnd.Models;
using Microsoft.Data.SqlClient;

namespace DrKnuffelBackEnd.Repositories.UserData;

public class SQLExtraUserData : IExtraUserData
{
    private readonly string SqlString;

    public SQLExtraUserData(string sqlConnectionString)
    {
        SqlString = sqlConnectionString;
    }
    
    public async Task AddAsync(Models.UserData data)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            await sqlConnection.ExecuteAsync("INSERT INTO [UserData] (Id, DoctorName, AppointmentDate, AppointmentType, UserAge, User_id) VALUES (@Id, @DoctorName, @AppointmentDate, @AppointmentType, @UserAge, @UserId)", data);
        }
    }

    public async Task<Models.UserData> GetAsync(Guid id)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QuerySingleOrDefaultAsync<Models.UserData>("SELECT * FROM [UserData] WHERE Id = @Id", new { id });   
        }
    }

    public async Task<Models.UserData> GetAsyncByUserId(string UID)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QuerySingleOrDefaultAsync<Models.UserData>("SELECT * FROM [UserData] WHERE User_id = @UID", new { UID });   
        };
    }
}