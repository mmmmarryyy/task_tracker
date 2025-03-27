using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Username", TypeName = "nvarchar(100)")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("PasswordHash", TypeName = "nvarchar(256)")]
        public string PasswordHash { get; set; } = string.Empty;

        public virtual UserProfile? Profile { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
