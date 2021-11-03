using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WaterResort.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        [Required]
        public bool QueenBeds { get; set; }
        [Required]
        public bool KingBeds { get; set; }
        [Required]
        public bool Reserved { get; set; }
        [Required]
        public bool Cleaned { get; set; }
        [Required]
        public bool LakeFacing { get; set; }
        [Required]
        public bool Suite { get; set; }
        public int RoomNumber { get; set; }
        public decimal CostPerNight { get; set; }
    }
}
