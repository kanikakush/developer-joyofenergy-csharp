using System;
using System.ComponentModel.DataAnnotations;
namespace JOIEnergy.Domain
{
    public class ElectricityReading
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Reading must be greater than 0")]
        public decimal Reading { get; set; }
    }
}
