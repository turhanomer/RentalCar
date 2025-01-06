using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Models
{
    [Table("Cars")]
    public class Arac
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Column("Brand")]
        public string Marka { get; set; } = string.Empty;
        
        [Required]
        [Column("Model")]
        public string Model { get; set; } = string.Empty;
        
        [Required]
        [Column("Year")]
        public int Yil { get; set; }
        
        [Required]
        [Column("DailyPrice")]
        public decimal GunlukFiyat { get; set; }
        
        [Column("ImageUrl")]
        public string? ResimUrl { get; set; }
        
        [Column("IsAvailable")]
        public bool Musait { get; set; } = true;

        [Required]
        [Column("Capacity")]
        public int Kapasite { get; set; }

        [Required]
        [Column("FuelType")]
        [StringLength(20)]
        public string YakitTuru { get; set; } = string.Empty;

        [Required]
        [Column("Transmission")]
        [StringLength(20)]
        public string VitesTuru { get; set; } = string.Empty;
    }
} 