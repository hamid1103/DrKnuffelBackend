using System.Data.SqlTypes;
using Dapper;
using DrKnuffelBackEnd.Models;
using Microsoft.Data.SqlClient;

namespace DrKnuffelBackEnd.Repositories;

public class SQLExtraUserData : IExtraUserData
{
    private readonly string SqlString;

    public SQLExtraUserData(string sqlConnectionString)
    {
        SqlString = sqlConnectionString;
    }
    
    public async Task AddAsync(UserData data)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            await sqlConnection.ExecuteAsync("INSERT INTO [UserData] (Id, DoctorName, AppointmentDate, AppointmentType, UserAge, UserId) VALUES (@Id, @DoctorName, @AppointmentDate, @AppointmentType, @UserAge, @UserId)", data);
        }
    }

    public async Task<UserData> GetAsync(Guid id)
    {
        using (var sqlConnection = new SqlConnection(SqlString))
        {
            return await sqlConnection.QuerySingleOrDefaultAsync<UserData>("SELECT * FROM [UserData] WHERE Id = @Id", new { id });   
        }
    }
}