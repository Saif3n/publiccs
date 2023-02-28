using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using React.Models;
using React.Dtos;


namespace React.Data
{
    public class ReactDBContext : DbContext
    {
        public ReactDBContext(DbContextOptions<ReactDBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Sponsors> Sponsors { get; set; }
        public DbSet<RaceResult> RaceResult { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<RestaurantUser> RestaurantUser { get; set; }
        public DbSet<Review> Review { get; set; }

    }

}