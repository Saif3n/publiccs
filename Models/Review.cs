using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace React.Models
{

    public class Review
    {
        public int reviewId { get; set; }

        public int timeWaited { get; set; }
        public string? requiredBooking { get; set; }

        public string weekDay { get; set; }
        public string reviewDate { get; set; }
        public string reviewTime { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public Restaurant Restaurant { get; set; }
        public RestaurantUser User { get; set; }
    }

}

