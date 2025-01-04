using Microsoft.EntityFrameworkCore;
using RentalCar.Models;

namespace RentalCar.Data
{
    public class VeriTabaniContext : DbContext
    {
        public VeriTabaniContext(DbContextOptions<VeriTabaniContext> options)
            : base(options)
        {
        }

        public DbSet<Arac> Araclar { get; set; }
        public DbSet<Yonetici> Yoneticiler { get; set; }
        public DbSet<KiralamaTalebi> KiralamaTalepleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Yönetici verilerini seed'le
            modelBuilder.Entity<Yonetici>().HasData(
                new Yonetici
                {
                    Id = 1,
                    KullaniciAdi = "admin",
                    Sifre = "admin123",
                    AdSoyad = "Sistem Yöneticisi",
                    Email = "admin@example.com"
                }
            );

            // Araç ve KiralamaTalebi ilişkisini tanımla
            modelBuilder.Entity<KiralamaTalebi>()
                .HasOne(r => r.Arac)
                .WithMany()
                .HasForeignKey(r => r.AracId)
                .OnDelete(DeleteBehavior.Cascade);

            // Arac.GunlukFiyat için hassasiyet tanımla
            modelBuilder.Entity<Arac>()
                .Property(c => c.GunlukFiyat)
                .HasColumnType("decimal(18,2)");
        }
    }
} 