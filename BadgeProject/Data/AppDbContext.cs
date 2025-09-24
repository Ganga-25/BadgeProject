using BadgeProject.Model;
using Microsoft.EntityFrameworkCore;

namespace BadgeProject.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserOtp>().ToTable("UserOtp");
        }


    }
}
