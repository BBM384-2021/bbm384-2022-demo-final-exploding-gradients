using Microsoft.EntityFrameworkCore;

namespace LinkedHU_CENG.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Administrator> Administrators { get; set; }

        public DbSet<UnregisteredUser> UnregisteredUsers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<BannedUser> BannedUsers { get; set; }
        public DbSet<DeleteRequest> DeleteRequests { get; set; }
        public DbSet<ForgetPassword> ForgetPasswords { get; set; }
        public DbSet<Chat> Chats { get; set; }

        // db bağlantısı için connection stringi burda giriyoruz
        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        => optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=LinkedHU_CENG;User Id=postgres;Password=burak2001");

        //
    }
}
