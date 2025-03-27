using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;

namespace TaskTracker.DataAccess
{
    public class EFTaskTrackerContext : DbContext
    {
        public EFTaskTrackerContext(DbContextOptions<EFTaskTrackerContext> options): base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<Domain.Entities.Task> Tasks { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tags)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasMany(t => t.Tags)
                .WithMany(tag => tag.Tasks)
                .UsingEntity(
                    "TaskTags",
                    l => l.HasOne(typeof(Tag))
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .HasPrincipalKey(nameof(Tag.Id))
                        .OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(typeof(Domain.Entities.Task))
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .HasPrincipalKey(nameof(Domain.Entities.Task.Id))
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j.HasKey("TasksId", "TagsId")
                );


            modelBuilder.Entity<Domain.Entities.Task>()
                .HasDiscriminator<string>("TaskType")
                .HasValue<Domain.Entities.Task>("Regular")
                .HasValue<RecurringTask>("Recurring");

            modelBuilder.Entity<Domain.Entities.Task>()
                .ToTable("Tasks")
                .InsertUsingStoredProcedure("sp_InsertTask", sp =>
                {
                    sp.HasParameter(t => t.Title);
                    sp.HasParameter(t => t.Description);
                    sp.HasParameter(t => t.CreatedDate);
                    sp.HasParameter(t => t.Deadline);
                    sp.HasParameter(t => t.Status);
                    sp.HasParameter(t => t.Priority);
                    sp.HasParameter(t => t.UserId);
                    sp.HasParameter(t => t.IsArchived);
                    sp.HasParameter("TaskType");
                    sp.HasParameter("RecurrenceInterval");
                    sp.HasResultColumn(t => t.Id);
                })
                .UpdateUsingStoredProcedure("sp_UpdateTask", sp =>
                {
                    sp.HasOriginalValueParameter(t => t.Id);
                    sp.HasParameter(t => t.Title);
                    sp.HasParameter(t => t.Description);
                    sp.HasParameter(t => t.CreatedDate);
                    sp.HasParameter(t => t.Deadline);
                    sp.HasParameter(t => t.Status);
                    sp.HasParameter(t => t.Priority);
                    sp.HasParameter(t => t.UserId);
                    sp.HasParameter(t => t.IsArchived);
                    sp.HasOriginalValueParameter("TaskType");
                    sp.HasParameter("RecurrenceInterval");
                })
                .DeleteUsingStoredProcedure("sp_DeleteTask", sp =>
                {
                    sp.HasOriginalValueParameter(t => t.Id);
                });
        }
    }
}
