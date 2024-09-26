using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace Cinema.Repository.Models
{
    public class Room : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string RoomNumber { get; set; }
    }
}
