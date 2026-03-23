namespace DrKnuffelBackEnd.Models;

public class UserData
{
    public Guid? Id { get; set; }
    public string? DoctorName { get; set; }
    public DateOnly? AppointmentDate { get; set; }

    public string? AppointmentType { get; set; }
    //In Years
    public int UserAge { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}