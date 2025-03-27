using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    [Table("Tasks")]
    public class Task
    {
        public Task()
        {
            this.Tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Title", TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Column("Description", TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [Column("Deadline")]
        public DateTime Deadline { get; set; }

        [Required]
        [Column("Status")]
        public Enums.TaskStatus Status { get; set; }

        [Required]
        [Column("Priority")]
        public Enums.TaskPriority Priority { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
