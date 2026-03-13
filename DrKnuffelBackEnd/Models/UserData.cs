namespace DrKnuffelBackEnd.Models;

public class UserData
{
    public Guid Id;
    public string DoctorName;
    public DateOnly AppointmentDate;

    public string AppointmentType;
    //In Years
    public int UserAge;
    public Guid UserId;
}