using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WaterResort.Models
{
    public class RestaurantTableReservation
    {
        public int Id { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
    }
}
