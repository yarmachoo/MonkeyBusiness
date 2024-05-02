using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.Models.Entity;

namespace MonkeyBuisness.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }
    }
}
