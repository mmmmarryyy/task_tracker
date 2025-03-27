using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskTracker.DataAccess
{
    // TODO: for creating migrations
    public class EFTaskTrackerContextFactory : IDesignTimeDbContextFactory<EFTaskTrackerContext>
    {
        public EFTaskTrackerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFTaskTrackerContext>();

            // TODO: move connections string to config
            optionsBuilder.UseSqlServer("Data Source=WRS-KIY-005\\SQLEXPRESS;Initial Catalog=TaskTrackerDB;Integrated Security=True;Encrypt=False"); //TODO: check that nowhere have Products

            return new EFTaskTrackerContext(optionsBuilder.Options);
        }
    }
}
