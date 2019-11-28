using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bingo.Models;

namespace Bingo.Controllers
{
    
    public class UserController : Controller
    {
        private BingoDbContext db = new BingoDbContext();

        // Check Email Exists or not in DB  
        private bool IsEmailExists(string eMail)
        {
            var IsCheck = db.Users.Where(email => email.EmailId == eMail).FirstOrDefault();
            return IsCheck != null;
        }

        // Check Username Exists or not in DB  
        private bool IsUserNameExists(string uName)
        {
            var IsCheck = db.Users.Where(uname => uname.UserName == uName).FirstOrDefault();
            return IsCheck != null;
        }


        // GET: User
        public ActionResult Index(int? id)
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                if(id == null)
                {
                    id = Int32.Parse(obj.ToString());
                }
                User user = db.Users.Find(id);
                return View("Index", user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult List(String searchString)
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                bool MalePreference = Boolean.Parse(Session["MalePreference"].ToString());
                bool FemalePreference = Boolean.Parse(Session["FemalePreference"].ToString());
                bool OtherPreference = Boolean.Parse(Session["OtherPreference"].ToString());
                string Gender = Session["Gender"].ToString();
                IEnumerable<User> matchedUsers = (from u in db.Users
                                    from m in db.Matches 
                                    where (u.UserId == uId) || (m.SenderId == uId && m.ReceiverId == u.UserId)
                                                            || (m.ReceiverId == uId && m.SenderId == u.UserId)
                                                            || (u.Gender == "Male" && !MalePreference) 
                                                            || (u.Gender == "Female" && !FemalePreference) 
                                                            || (u.Gender == "Other" && !OtherPreference)
                                                            || (Gender == "Male" && !u.MalePreference)
                                                            || (Gender == "Female" && !u.FemalePreference)
                                                            || (Gender == "Other" && !u.OtherPreference)
                                                  select u).ToList().Distinct();
                IEnumerable<User> users;
                IEnumerable<User> searches;
                if (!String.IsNullOrEmpty(searchString))
                {
                    searches = db.Users.Where(u => u.UserName.Contains(searchString) && u.UserId != uId).ToList();
                    searches = searches.Except(matchedUsers);
                    return View(searches);
                }

                if (matchedUsers.Count() != 0)
                {
                    users = (from u in db.Users select u).ToList();
                    System.Console.WriteLine(users.ToList().Count);
                    users = users.Except(matchedUsers);
                    System.Console.WriteLine(matchedUsers.ToList().Count);
                    System.Console.WriteLine(users.ToList().Count);
                }
                else
                {
                    users = db.Users.Where(u => u.UserId != uId).ToList();
                }
                users = sort(users);
                return View(users);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //Signup
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User user)
        {
            var IsExistsEmail = IsEmailExists(user.EmailId);
            if (IsExistsEmail)
            {
                ModelState.AddModelError("EmailExists", "Email Already Exists");
            }

            var IsExistsUserName = IsUserNameExists(user.UserName);
            if (IsExistsUserName)
            {
                ModelState.AddModelError("UsernameExists", "Username Already Exists");
            }
            if (ModelState.IsValid)
            {
                user.MalePreference = true;
                user.FemalePreference = true;
                user.OtherPreference = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
        //Edit User
        public ActionResult Edit()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                User user = db.Users.Find(uId);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file1)
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                int uId = Int32.Parse(obj.ToString());
                var userToUpdate = db.Users.Find(uId);
                if (TryUpdateModel(userToUpdate, "", new string[] {
                    "UserName", "FirstName", "LastName", "DisplayBirthdate", "City", "Occupation",
                    "ProfilePicture", "Bio", "Likes", "Dislikes", "Hobbies", "Contact", "MalePreference", 
                    "FemalePreference", "OtherPreference" }))
                {
                    try
                    {
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            userToUpdate.ProfilePicture = new byte[file1.ContentLength];
                            file1.InputStream.Read(userToUpdate.ProfilePicture, 0, file1.ContentLength);
                        }
                        db.SaveChanges();

                        User u = db.Users.Find(uId);
                        Session["MalePreference"] = u.MalePreference.ToString();
                        Session["FemalePreference"] = u.FemalePreference.ToString();
                        Session["OtherPreference"] = u.OtherPreference.ToString();

                        return RedirectToAction("Index", "User", null);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        throw e;
                    }
                }
            }
            return View(User);
        }
        //Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var get_user = db.Users.SingleOrDefault(p => p.UserName == user.UserName
            && p.Password == user.Password);
            if (get_user != null)
            {
                Session["UserId"] = get_user.UserId.ToString();
                Session["MalePreference"] = get_user.MalePreference.ToString();
                Session["FemalePreference"] = get_user.FemalePreference.ToString();
                Session["OtherPreference"] = get_user.OtherPreference.ToString();
                Session["Gender"] = get_user.Gender.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password does not match.");
            }

            return View();
        }
        public ActionResult Logout()
        {

            object obj = Session["UserId"];
            if (obj != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Index", "Home", null);
        }

        private IEnumerable<User> sort(IEnumerable<User> users)
        {
            int uId = Int32.Parse(Session["UserId"].ToString());
            User currentUser = db.Users.Find(uId);
            List<Int32> scores = new List<Int32>();
            int score;
            foreach (User user in users)
            {
                score = 0;
                if (currentUser.Gender != null)
                {
                    if (user.Gender == null)
                    {
                        score += 20;
                    } else if ((currentUser.Gender != "Other" && user.Gender != currentUser.Gender)
                            || (currentUser.Gender == "Other" && user.Gender == "Other"))
                    {
                        score += 50;
                    }
                }
                if (currentUser.Occupation != null && user.Occupation != null &&
                    JaroWinklerDistance.proximity(currentUser.Occupation, user.Occupation) >= 0.8)
                {
                    score += 10;
                }
                if (currentUser.City != null && user.City != null &&
                    JaroWinklerDistance.proximity(currentUser.City, user.City) >= 0.8)
                {
                    score += 10;
                }
                if (currentUser.Likes != null && user.Likes != null)
                {
                    score += matchWordsScore(currentUser.Likes, user.Likes);
                }
                if (currentUser.Dislikes != null && user.Dislikes != null)
                {
                    score += matchWordsScore(currentUser.Dislikes, user.Dislikes);
                }
                if (currentUser.Hobbies != null && user.Hobbies != null)
                {
                    score += matchWordsScore(currentUser.Hobbies, user.Hobbies);
                }
                if (currentUser.Bio != null && user.Bio != null)
                {
                    score += matchWordsScore(currentUser.Bio, user.Bio);
                }
                scores.Add(score);
            }
            var orderedZip = scores.Zip(users, (x, y) => new { x, y })
                                    .OrderByDescending(pair => pair.x)
                                    .ToList();
            users = orderedZip.Select(pair => pair.y).ToList();
            return users;
        }

        private int matchWordsScore(string search, string part)
        {
            System.Console.WriteLine(search, part);
            var s = search.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int count = part.Split(new char[] { ' ' }).Sum(p => s.Contains(p.ToLower()) ? 1 : 0);
            int score = 0;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    score += 5;
                    break;
                case 2:
                    score += 10;
                    break;
                case 3:
                    score += 15;
                    break;
                default:
                    score += 20;
                    break;
            }
            return score;
        }
    }
}

static class JaroWinklerDistance
{
    /* The Winkler modification will not be applied unless the 
        * percent match was at or above the mWeightThreshold percent 
        * without the modification. 
        * Winkler's paper used a default value of 0.7
        */
    private static readonly double mWeightThreshold = 0.7;

    /* Size of the prefix to be concidered by the Winkler modification. 
        * Winkler's paper used a default value of 4
        */
    private static readonly int mNumChars = 4;


    /// <summary>
    /// Returns the Jaro-Winkler distance between the specified  
    /// strings. The distance is symmetric and will fall in the 
    /// range 0 (perfect match) to 1 (no match). 
    /// </summary>
    /// <param name="aString1">First String</param>
    /// <param name="aString2">Second String</param>
    /// <returns></returns>
    public static double distance(string aString1, string aString2)
    {
        return 1.0 - proximity(aString1, aString2);
    }


    /// <summary>
    /// Returns the Jaro-Winkler distance between the specified  
    /// strings. The distance is symmetric and will fall in the 
    /// range 0 (no match) to 1 (perfect match). 
    /// </summary>
    /// <param name="aString1">First String</param>
    /// <param name="aString2">Second String</param>
    /// <returns></returns>
    public static double proximity(string aString1, string aString2)
    {
        int lLen1 = aString1.Length;
        int lLen2 = aString2.Length;
        if (lLen1 == 0)
            return lLen2 == 0 ? 1.0 : 0.0;

        int lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

        // default initialized to false
        bool[] lMatched1 = new bool[lLen1];
        bool[] lMatched2 = new bool[lLen2];

        int lNumCommon = 0;
        for (int i = 0; i < lLen1; ++i)
        {
            int lStart = Math.Max(0, i - lSearchRange);
            int lEnd = Math.Min(i + lSearchRange + 1, lLen2);
            for (int j = lStart; j < lEnd; ++j)
            {
                if (lMatched2[j]) continue;
                if (aString1[i] != aString2[j])
                    continue;
                lMatched1[i] = true;
                lMatched2[j] = true;
                ++lNumCommon;
                break;
            }
        }
        if (lNumCommon == 0) return 0.0;

        int lNumHalfTransposed = 0;
        int k = 0;
        for (int i = 0; i < lLen1; ++i)
        {
            if (!lMatched1[i]) continue;
            while (!lMatched2[k]) ++k;
            if (aString1[i] != aString2[k])
                ++lNumHalfTransposed;
            ++k;
        }
        // System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
        int lNumTransposed = lNumHalfTransposed / 2;

        // System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
        double lNumCommonD = lNumCommon;
        double lWeight = (lNumCommonD / lLen1
                            + lNumCommonD / lLen2
                            + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0;

        if (lWeight <= mWeightThreshold) return lWeight;
        int lMax = Math.Min(mNumChars, Math.Min(aString1.Length, aString2.Length));
        int lPos = 0;
        while (lPos < lMax && aString1[lPos] == aString2[lPos])
            ++lPos;
        if (lPos == 0) return lWeight;
        return lWeight + 0.1 * lPos * (1.0 - lWeight);

    }
}