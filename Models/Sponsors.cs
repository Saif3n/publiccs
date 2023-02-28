using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React.Models
{
    public class Sponsors
    {
        [Key]
        public int sponsorID { get; set; }
        public string? sponsorName { get; set; }
        public string? sponsorLink { get; set; }
        public string? sponsorStockName { get; set; }
        public int? sponsorYear { get; set; }
        public string? teamName { get; set; }
        public int? isCrypto { get; set; }


    }

}