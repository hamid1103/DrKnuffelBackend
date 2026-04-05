namespace DrKnuffelBackEnd.Models
{
    public class Progress
    {
        public Guid? Id { get; set; }
        public Guid? UserData_id { get; set; }
        public Guid? Step_id { get; set; }
        public bool Completed { get; set; }
        public DateTime Completed_at { get; set; }
        public int? StepOrder;


    }
}
