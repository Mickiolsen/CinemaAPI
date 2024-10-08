using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;
using xxx.Repository.Models;

namespace Cinema.Repository.Models
{
    public class Country : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        [JsonIgnore]
        public List<Actor> Actors { get; set; } = [];
    }
}
