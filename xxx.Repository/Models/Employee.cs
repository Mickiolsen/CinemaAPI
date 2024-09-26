using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxx.Repository.Interfaces;

namespace Cinema.Repository.Models
{
    public class Employee : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }  
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
