using Web2ProjectMVC.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web2ProjectMVC.Repository
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<UserToAd> UserStatuses { get; set; }

        public ProjectDbContext()
        {
            Users = Set<User>();
            Ads = Set<Ad>();
            UserStatuses = Set<UserToAd>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-UB31SED\\SQLEXPRESS;Database=ProjectLastVersionMVC;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                UserName = "oblakov",
                Password = "oblakov",
                FirstName = "Demian",
                LastName = "Oblakov",
                Email = "demaoblak1@gmail.com",
                Role = Roles.Admin,
            });
        }
    }
}
