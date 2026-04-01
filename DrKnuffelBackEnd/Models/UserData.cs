using System.Text.Json.Serialization;

namespace DrKnuffelBackEnd.Models;

public class UserData
{
    public Guid? Id { get; set; }
    public string? DoctorName { get; set; }
    public DateTime? AppointmentDate { get; set; }

    public string? AppointmentType { get; set; }
    //In Years
    public int UserAge { get; set; }

    [JsonIgnore]
    public string? UserId { get; set; }

    public string? RoleId { get; set; }
}