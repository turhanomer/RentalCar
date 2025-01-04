using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Models
{
    [Table("Admins")]
    public class Yonetici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Username")]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        [Column("Password")]
        public string Sifre { get; set; } = string.Empty;

        [Required]
        [Column("FullName")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;
    }
} 