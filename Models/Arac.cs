using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RentalCar.Models
{
    [Table("Cars")]
    public class Arac
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Marka alanı zorunludur")]
        [Column("Brand")]
        [StringLength(50)]
        public string Marka { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Model alanı zorunludur")]
        [Column("Model")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Yıl alanı zorunludur")]
        [Column("Year")]
        [Range(1900, 2024, ErrorMessage = "Yıl 1900-2024 arasında olmalıdır")]
        public int Yil { get; set; }
        
        [Required(ErrorMessage = "Günlük fiyat alanı zorunludur")]
        [Column("DailyPrice")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal GunlukFiyat { get; set; }
        
        [Column("ImageUrl")]
        public string? ResimUrl { get; set; }
        
        [NotMapped]
        [Display(Name = "Araç Resmi")]
        public IFormFile? ResimDosyasi { get; set; }
        
        [Column("IsAvailable")]
        public bool Musait { get; set; } = true;

        [Required(ErrorMessage = "Kapasite alanı zorunludur")]
        [Column("Capacity")]
        [Range(1, 50, ErrorMessage = "Kapasite 1-50 kişi arasında olmalıdır")]
        public int Kapasite { get; set; }

        [Required(ErrorMessage = "Yakıt türü seçilmelidir")]
        [Column("FuelType")]
        [StringLength(20)]
        public string YakitTuru { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vites türü seçilmelidir")]
        [Column("Transmission")]
        [StringLength(20)]
        public string VitesTuru { get; set; } = string.Empty;

        [NotMapped]
        public string DurumText => Musait ? "Müsait" : "Kirada";

        [NotMapped]
        public string DurumClass => Musait ? "success" : "danger";
    }
} 