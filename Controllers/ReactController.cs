using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using React.Models;
using React.Data;
using System.Web.Http.Cors;
using React.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;


namespace React.Controllers
{

    [EnableCors("*", "*", "*")]
    [ApiController]

    public class ReactController : Controller
    {

        private readonly IReactRepo _repo;
        public ReactController(IReactRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("Register")]
        public ActionResult<String> Register(UserIn register)
        {

            string returnLine = "User successfully registered.";

            User data = new User { Name = register.Name, Message = register.Message, Email = register.Email };
            User addUser = _repo.AddUser(data);

            return Ok(returnLine);
        }

        [HttpGet("GetLastUser")]
        public ActionResult<UserOut> GetLastUser()
        {

            User userMatch = _repo.GetLastUser();
            UserOut userOut = new UserOut { Name = userMatch.Name };
            return Ok(userOut);
        }

        
        //[HttpGet("wIdjs90218fjIOownvkdfoL")]
        //public ActionResult<User> GetAll()
        //{

        //    IEnumerable<User> userMatch = _repo.GetAll();
        //    return Ok(userMatch);
        //}


        [HttpGet("GetAllSponsors")]
        public ActionResult<Sponsors> GetSponsors()
        {

            IEnumerable<Sponsors> userMatch = _repo.GetAllSponsors();
            return Ok(userMatch);
        }


        [HttpGet("GetResultByTeam")]
        public ActionResult<RaceResult> Get(string teamName, string? race = null)
        {

            IEnumerable<RaceResult> userMatch = _repo.GetDriverResult(teamName, race);
            return Ok(userMatch);
        }

        [HttpGet("WakeUp")]
        public ActionResult<String> WakeFunction()
        {
            return Ok();
        }

        /* ---------------- this was used as a search bar, but it caused repeated requests, so it's now redundant. -------------*/

        //[HttpGet("GetSponsorByName/{name}")]
        //public ActionResult<Sponsors> GetSponsorByName(string name)
        //{

        //    IEnumerable<Sponsors> sponsor = _repo.GetSponsorByName(name);

        //    if (sponsor == null)
        //        return NotFound();
        //    else
        //    {
        //        return Ok(sponsor);
        //    }

        //}

        //[HttpGet("GetAllResults")]
        //public ActionResult<RaceResult> GetResults()
        //{

        //    IEnumerable<RaceResult> userMatch = _repo.GetAllResults();
        //    return Ok(userMatch);
        //}

        /* ---------------- FOOD APP -------------*/
        [HttpPost("RestuarantUserRegister")]
        public ActionResult<String> RegisterRestaurantUser(RestaurantUserIn user)
        {
            string md5HashPassword = _repo.hashMD5(user.password);
            DateTime today = DateTime.Today;
            string success = "Successfully registered.";
            string fail = "Registration unsuccessful, an account already exists with that name.";

            RestaurantUser data = new RestaurantUser { userName = user.userName, password = md5HashPassword, emailAddress = user.emailAddress, dateJoined = today.ToString() };
            bool addRestaurantUser = _repo.AddRestaurantUser(data);

            if (addRestaurantUser)
            {
                return Ok(success);
            }

            return Conflict(fail);

        }


        // this method would only work in a "change password in settings" feature. Does not work if the user does not know the old password
        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpPost("ChangePasswordLoggedIn")]

        public ActionResult<String> ChangePass([FromBody] string newPassword)
        {
            ClaimsIdentity versUser = HttpContext.User.Identities.FirstOrDefault();
            Claim user = versUser.FindFirst("normalUser");
            Claim pass = versUser.FindFirst("password");

            string success = "Password changed successfully.";
            string fail = "Your old password did not match.";

            bool retString = _repo.UpdateRestaurantUser(user.Value, pass.Value, newPassword);

            if (retString)
            {
                return Ok(success);
            }
            return Conflict(fail);

        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpPost("AddReview")]
        public ActionResult<Review> AddReview(ReviewIn review)
        {
            ClaimsIdentity versUser = HttpContext.User.Identities.FirstOrDefault();
            Claim user = versUser.FindFirst("normalUser");
            string success = "Review added successfully";

            DateTime date = DateTime.ParseExact(review.reviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string DayOfWeek = date.DayOfWeek.ToString();

            Review data = new Review {
                UserId = _repo.GetRestaurantUserId(user.Value),
                RestaurantId = review.RestaurantId,
                weekDay = DayOfWeek,
                timeWaited = review.timeWaited,
                requiredBooking = review.requiredBooking,
                reviewDate = review.reviewDate,
                reviewTime = review.reviewTime
            };

            _repo.AddReview(data);

            return Ok(success);
        }

    }
}