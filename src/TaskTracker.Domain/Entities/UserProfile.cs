using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities
{
    [Table("UserProfiles")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("FullName", TypeName = "nvarchar(200)")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Column("DateOfBirth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Column("Bio", TypeName = "nvarchar(max)")]
        public string Bio { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
