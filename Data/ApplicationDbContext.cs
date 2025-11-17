using Microsoft.EntityFrameworkCore;
using GrapheneTrace.Models;

namespace GrapheneTrace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("longtext");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasColumnType("varchar(50)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
