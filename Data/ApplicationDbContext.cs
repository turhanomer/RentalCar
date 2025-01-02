using Microsoft.EntityFrameworkCore;
using RentalCar.Models;

namespace RentalCar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin verilerini seed'le
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    FullName = "System Admin",
                    Email = "admin@example.com"
                }
            );

            // Car ve RentalRequest ilişkisini tanımla
            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.Car)
                .WithMany()
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Car.DailyPrice için hassasiyet tanımla
            modelBuilder.Entity<Car>()
                .Property(c => c.DailyPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
} 