using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities
{
    [Table("Tags")]
    public class Tag
    {
        public Tag()
        {
            this.Tasks = new HashSet<Entities.Task>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
