namespace DrKnuffelBackEnd.Models
{
    public class Progress
    {
        public Guid? Id { get; set; }
        public Guid UserDataId { get; set; }
        public Guid StepId { get; set; }
        public bool Completed { get; set; }
        public DateTime Completed_at { get; set; }


    }
}
