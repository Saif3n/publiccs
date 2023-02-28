using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using React.Models;
using System.Security.Cryptography;
using System.Text;

namespace React.Data
{
    public class ReactRepo : IReactRepo
    {
        private readonly ReactDBContext _dbContext;
        public ReactRepo(ReactDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddUser(User user)
        {

            EntityEntry<User> userToAdd = _dbContext.Users.Add(user);
            User add = userToAdd.Entity;
            _dbContext.SaveChanges();
            return add;
        }


        public User GetUser(string username)
        {
            User user = _dbContext.Users.FirstOrDefault(e => e.Name == username);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = _dbContext.Users.ToList<User>();
            return users;
        }
        public User GetLastUser()
        {
            User user = _dbContext.Users.OrderBy(e => e.Id).LastOrDefault();
            return user;

        }


        /* UNUSED, this causes repeated requests. */
        public IEnumerable<Sponsors> GetSponsorByName(string sponsorName)
        {
            string sponsorLower = sponsorName.ToLower();
            IEnumerable<Sponsors> sponsor = _dbContext.Sponsors.Where(Sponsors => Sponsors.sponsorName.ToLower().Contains(sponsorLower));
            return sponsor;
        }


        /* -------------%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%----------Related to stock website (in-dev)-------%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%------*/

        public IEnumerable<Sponsors> GetAllSponsors()
        {
            IEnumerable<Sponsors> sponsor = _dbContext.Sponsors.ToList<Sponsors>();
            return sponsor;
        }

        /* OPTIONAl*/
        public IEnumerable<RaceResult> GetAllResults()
        {
            IEnumerable<RaceResult> results = _dbContext.RaceResult.ToList<RaceResult>();
            return results;
        }


        public IEnumerable<RaceResult> GetDriverResult(string teamName, string race)
        {
            string teamLower = teamName.ToLower();


            if (race == null)
            {
                IEnumerable<RaceResult> results = _dbContext.RaceResult.Where(RaceResult => RaceResult.teamName.ToLower().Contains(teamLower));
                return results;
            }

            string raceLower = race.ToLower();

            IEnumerable<RaceResult> resultsDriver = _dbContext.RaceResult.Where(RaceResult => RaceResult.teamName.ToLower().Contains(teamLower) && RaceResult.race.ToLower().Contains(raceLower));

            return resultsDriver;
        }


        /*-----------%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%-------RESTAURANT----------%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%-------*/

        /*---AUTHENTICATION ---*/
        public string hashMD5(string hashValue)
        {

            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(hashValue);
            byte[] hash = md5.ComputeHash(inputBytes);

            string hashString = BitConverter.ToString(hash).Replace("-", "");

            return hashString.ToLower();
        }

        public bool CheckUser(string userName, string password)
        {

            string hashCrossCheck = hashMD5(password);

            RestaurantUser c = _dbContext.RestaurantUser.FirstOrDefault(e => e.userName == userName && e.password == hashCrossCheck);

            return c != null;

        }


        /*---AUTHENTICATION ---*/

        public bool AddRestaurantUser(RestaurantUser user)
        {

            RestaurantUser userDuplicate = _dbContext.RestaurantUser.FirstOrDefault(e => e.userName == user.userName);
            if (userDuplicate == null)
            {
                EntityEntry<RestaurantUser> userToAdd = _dbContext.RestaurantUser.Add(user);
                RestaurantUser add = userToAdd.Entity;
                _dbContext.SaveChanges();
                return true;
            }

            return false;

        }
        public int GetRestaurantUserId(string username)
        {
            RestaurantUser user = _dbContext.RestaurantUser.FirstOrDefault(e => e.userName == username);
            return user.userId;
        }

      
          // this method would only work in a "change password in settings" feature. Does not work if the user does not know the old password
        public bool UpdateRestaurantUser(string userName, string oldPass, string newPassword)
        {
            RestaurantUser c = _dbContext.RestaurantUser.FirstOrDefault(e => e.userName == userName && e.password == hashMD5(oldPass));

            if (c != null)
            {
                c.password = hashMD5(newPassword);
                _dbContext.RestaurantUser.Update(c);
                _dbContext.SaveChanges();

                return true;
            }
            return false;

        }

        public void AddReview (Review review)
        {
            EntityEntry<Review> reviewAdd = _dbContext.Review.Add(review);
            Review add = reviewAdd.Entity;
            _dbContext.SaveChanges();

        }



    }
}
