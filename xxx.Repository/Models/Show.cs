using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;

namespace Cinema.Repository.Models
{
    public class Show : IEntity
    {
        public int Id { get; set; }

        [Required]
        public DateOnly Date {  get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public int Price { get; set; }


        public int RoomId { get; set; }
        public Room room { get; set; }

        public int MovieId { get; set; }
        public Movie movie { get; set; }

    }
}
