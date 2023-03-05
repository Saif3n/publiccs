using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using React.Dtos;
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


        /* This was used as a search functionality, but calling the endpoint every onInput causes unneccessary requests. */
        public IEnumerable<Sponsors> GetSponsorByName(string sponsorName)
        {
            string sponsorLower = sponsorName.ToLower();
            IEnumerable<Sponsors> sponsor = _dbContext.Sponsors.Where(Sponsors => Sponsors.sponsorName.ToLower().Contains(sponsorLower));
            return sponsor;
        }


        /* --------------Related to stock website (in-dev)-------------*/

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


        public ReviewOut OutputReviewTime(Review time)
        {
            string format = "ddd MMM dd yyyy HH:mm:ss 'New Zealand Standard Time'";
            string returnString = "";
            string waitTime = CalculateWaitTime(time.timeWaited);

            DateTime reviewDateTime = DateTime.ParseExact(time.reviewTime, format, CultureInfo.InvariantCulture);
            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            DateTime nzdtReviewDateTime = TimeZoneInfo.ConvertTimeFromUtc(reviewDateTime.ToUniversalTime(), nzTimeZone);
            TimeSpan timeSinceReview = DateTimeOffset.Now - nzdtReviewDateTime;

            if (timeSinceReview.TotalMinutes <= 1)
            {
                returnString = "less than a minute ago";
            }
            else if (timeSinceReview.TotalMinutes < 60)
            {
                // Less than 1 hour ago
                int timeSinceLastReview = (int)timeSinceReview.TotalMinutes;
                returnString = timeSinceLastReview + " minutes ago";

            }
            else if (timeSinceReview.TotalHours < 24)
            {
                // Less than 1 day ago
                int hours = (int)timeSinceReview.TotalHours;
                int minutes = timeSinceReview.Minutes;
                returnString = $"{hours} hr {minutes} minutes ago";

            }
            else
            {
                // More than 1 day ago
                int days = (int)timeSinceReview.TotalDays;
                returnString = $"{days} days ago";
            }

            ReviewOut a = new ReviewOut { timeSinceLastReview = returnString, timeWaited = waitTime };

            return a;
        }

        public string CalculateWaitTime(int timeWaited)
        {
            string waitTime = "";

            if (timeWaited <= 1)
            {
                waitTime = "less than a minute";
            }
            else if (timeWaited <= 60)
            {
                waitTime = $"{timeWaited} minutes";
            }
            else if (timeWaited > 60)
            {
                int calcWaitMin = timeWaited % 60;

                int calcWaitHour = timeWaited / 60;

                waitTime = $"{calcWaitHour} hr {calcWaitMin} minute(s)";
            }

            return waitTime;
        }

        public async Task<Dictionary<string, WeekdayOut>> GetAverageTimeWeekdays(string companyName)
        {
            int companyId = await GetCompanyIdByName(companyName);
            Dictionary<string, WeekdayOut> results = new Dictionary<string, WeekdayOut>();

            foreach (var weekday in Enum.GetValues(typeof(DayOfWeek)))
            {
                string weekdayName = weekday.ToString();

                List<Review> reviews = await _dbContext.Review
                    .Where(Review => Review.CompanyId == companyId && Review.weekDay.ToLower() == weekdayName.ToLower())
                    .ToListAsync();

                if (reviews.Count == 0)
                {
                    WeekdayOut b = new WeekdayOut
                    {
                        avgWaitTime = "We unfortunately do not have enough info for this day, please help us by adding a review!",
                        numberOfReviews = ""
                    };

                    results.Add(weekdayName, b);
                }
                else
                {
                    int avgWaitTime = (int)reviews.Average(r => r.timeWaited);
                    WeekdayOut a = new WeekdayOut
                    {
                        avgWaitTime = CalculateWaitTime(avgWaitTime),
                        numberOfReviews = "Based on the average wait time from " + reviews.Count.ToString() + " review(s)"
                    };

                    results.Add(weekdayName, a);
                }
            }

            return results;
        }


    }
}
