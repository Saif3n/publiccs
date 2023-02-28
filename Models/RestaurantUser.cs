using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React.Models
{
    public class RestaurantUser
    {
        [Key]
        public int userId { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? dateJoined { get; set; }
        public string? emailAddress { get; set; }

    }

}