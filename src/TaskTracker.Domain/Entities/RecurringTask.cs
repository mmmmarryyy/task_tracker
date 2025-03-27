using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities
{
    public class RecurringTask : Task
    {
        [Column("RecurrenceInterval")]
        public TimeSpan RecurrenceInterval { get; set; }
    }
}
