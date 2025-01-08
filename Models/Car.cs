using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RentalCar.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka alanı zorunludur")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model alanı zorunludur")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yıl alanı zorunludur")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Kilometre alanı zorunludur")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Araç Resmi")]
        public IFormFile? ImageFile { get; set; }
    }
} 