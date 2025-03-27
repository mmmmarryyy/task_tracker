namespace TaskTracker.Domain.Entities
{
    public class Comment // TODO: delete this class later
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int TaskId { get; set; }
        public Entities.Task Task { get; set; }
    }
}
