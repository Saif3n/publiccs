using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React.Models
{
    public class Restaurant
    {
        [Key]
        public int restaurantID { get; set; }
        public string? restaurantName { get; set; }
        public string? restaurantStreetAddress { get; set; }
        public string? restaurantSuburb { get; set; }
        public string? restaurantCuisine { get; set; }
        public string? priceTier { get; set; }


    }

}