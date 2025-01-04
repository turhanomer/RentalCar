using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Models
{
    [Table("RentalRequests")]
    public class KiralamaTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("CarId")]
        public int AracId { get; set; }

        [Required]
        [Column("CustomerName")]
        [StringLength(100)]
        public string MusteriAdi { get; set; } = string.Empty;

        [Required]
        [Column("CustomerEmail")]
        [StringLength(100)]
        public string MusteriEmail { get; set; } = string.Empty;

        [Required]
        [Column("CustomerPhone")]
        [StringLength(20)]
        public string MusteriTelefon { get; set; } = string.Empty;

        [Required]
        [Column("StartDate")]
        public DateTime BaslangicTarihi { get; set; }

        [Required]
        [Column("EndDate")]
        public DateTime BitisTarihi { get; set; }

        [Required]
        [Column("RequestDate")]
        public DateTime TalepTarihi { get; set; }

        [Required]
        [Column("Status")]
        [StringLength(20)]
        public string Durum { get; set; } = "Pending";

        public Arac? Arac { get; set; }
    }
} 