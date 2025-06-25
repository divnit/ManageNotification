using ManageNotification.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageNotification.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<Reference> References { get; set; }
    }
}
