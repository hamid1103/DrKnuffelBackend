namespace DrKnuffelBackEnd.Models
{
    public class Step
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Step_order { get; set; }
    }
}
