using Microsoft.EntityFrameworkCore;

namespace NotesApi.Models
{
    public class MigrationDBContext(DbContextOptions<MigrationDBContext> options) : DbContext(options)
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}