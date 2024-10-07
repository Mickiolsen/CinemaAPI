using Cinema.Repository.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public string? TrailerLink { get; set; }
        [Required]
        public bool IsPopular { get; set; }
        [Required]
        public string Description { get; set; }
 
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        [JsonIgnore]
        public List<Actor>? Actors { get; set; }

        [JsonIgnore]
        public List<Show>? Shows { get; set; }

        public int? GenreId { get; set; }
       // [JsonIgnore]
       // public Genre? genre { get; set; }
    }
}
