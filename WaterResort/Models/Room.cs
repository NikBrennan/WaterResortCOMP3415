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
        public int AccountId { get; set; }
        [Required]
        public int QueenBeds { get; set; }
        [Required]
        public int KingBeds { get; set; }
        [Required]
        public bool Reserved { get; set; }
        [Required]
        public bool Cleaned { get; set; }
        [Required]
        public bool LakeFacing { get; set; }
        [Required]
        public bool Suite { get; set; }
    }
}
