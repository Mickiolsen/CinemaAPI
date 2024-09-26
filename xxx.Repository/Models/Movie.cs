using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace xxx.Repository.Models
{
    public class Movie : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public float DurationMinutes { get; set; }

        public string TrailerLink { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }

        //public int? GenreId { get; set; }

       // public Genre Genre { get; set; }
    }
}
