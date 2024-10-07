using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace Cinema.Repository.Models
{
    public class Seat : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string SeatRow { get; set; }
        [Required]
        public int SeatNumber { get; set; }


        public int RoomId { get; set; }
        public Room room { get; set; }
    }
}
