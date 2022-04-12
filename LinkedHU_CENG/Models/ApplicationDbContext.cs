using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        // db bağlantısı için connection stringi burda giriyoruz
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("User ID=postgres;Password=12345;Server=localhost;Port=5432;Database=UserDB;Integrated Security=true;Pooling=true;");

    }
}
