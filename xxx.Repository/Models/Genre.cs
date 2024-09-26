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
    public class Genre : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Genretype { get; set; }

        [JsonIgnore]
        public List<Movie> Movies { get; set; }

    }
}
