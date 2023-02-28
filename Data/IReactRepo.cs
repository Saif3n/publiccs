using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using React.Models;

namespace React.Data
{
    public interface IReactRepo
    {
        User AddUser(User user);

        User GetUser(string username);

        User GetLastUser();

        /* --------------Related to personal website-------------*/
        IEnumerable<User> GetAll();


        /* --------------Related to stock website (in-dev)-------------*/
        IEnumerable<Sponsors> GetAllSponsors();

        IEnumerable<RaceResult> GetAllResults();

        IEnumerable<RaceResult> GetDriverResult(string teamName, string race);

        IEnumerable<Sponsors> GetSponsorByName(string sponsorName);

        /*----Related to restaurant app---*/

        bool AddRestaurantUser(RestaurantUser restaurantUser);
        int GetRestaurantUserId(string username);
        bool UpdateRestaurantUser(string userName, string oldPass, string newPassword);
        void AddReview(Review review);

        /* --Related to authentication -- */
        string hashMD5(string hashValue);
        bool CheckUser(string userName, string password);





    }
}