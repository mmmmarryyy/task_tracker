using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class Task
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = "";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime Deadline { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public Enums.TaskPriority Priority { get; set; }
        public required int UserId { get; set; }
    }
}