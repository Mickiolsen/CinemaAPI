﻿using Cinema.Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string TrailerLink { get; set; }
        [Required]
        public bool IsPopular { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }


        [JsonIgnore]
        public List<Actor> Actors { get; set; }

        [JsonIgnore]
        public List<Show> Shows { get; set; }

        public int GenreId { get; set; }
    }
}
