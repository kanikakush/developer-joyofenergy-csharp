using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JOIEnergy.Domain
{
    public class MeterReadings
    {
        [Required(ErrorMessage = "SmartMeterId is required")]
        public string SmartMeterId { get; set; }

        [Required(ErrorMessage = "ElectricityReadings are required")]
        [MinLength(1, ErrorMessage = "At least one reading is required")]
        public List<ElectricityReading> ElectricityReadings { get; set; }
    }
}
