using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WaterResort.Models
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [Required]
        public bool Reserved { get; set; }
    }
}
