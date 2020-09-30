using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeatherWalkingSkeleton.Core
{
    public class CityTemperature 
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CityName { get ; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public decimal Temperature { get; set; }
        
    }
}
