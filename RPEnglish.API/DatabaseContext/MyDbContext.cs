using Microsoft.EntityFrameworkCore;
using RPEnglish.API.Entities;

namespace RPEnglish.API.DatabaseContext
{
    public class MyDbContext : DbContext
    {
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UsersPassword { get; set; }
        public DbSet<Word> Words { get; set; }
        
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    }
}