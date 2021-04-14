using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpAPI.Models
{
    public class MeetUpDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Organizer { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public bool? IsPrivate { get; set; }
    }
}
