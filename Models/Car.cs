using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCar.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Brand { get; set; } = string.Empty;
        
        [Required]
        public string Model { get; set; } = string.Empty;
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public decimal DailyPrice { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public string? Description { get; set; }
    }
} 