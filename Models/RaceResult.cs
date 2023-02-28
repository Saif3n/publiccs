using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React.Models
{
    public class RaceResult
    {
        [Key]
        public int resultID { get; set; }
        public string? racePosition { get; set; }
        public string? driver { get; set; }
        public string? teamName { get; set; }
        public string? race { get; set; }
        public string? raceDate { get; set; }

    }

}