using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace Cinema.Repository.Models
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public int SeatId { get; set; }
        public int ShowId { get; set; }
    }
}
