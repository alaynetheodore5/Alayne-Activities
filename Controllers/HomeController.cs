using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }
        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>();

        public  User GetUser()
        {
            return _context.Users.FirstOrDefault( u =>  u.UserId == HttpContext.Session.GetInt32("userId"));
        }

        public HomeController(MyContext context)
        {
            _context = context; 
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.FirstOrDefault(usr => usr.Email == u.Email) !=null)
                {
                    ModelState.AddModelError("Email", "Email is already in use, try logging in!");
                    return View("Index");
                }
                string hash = regHasher.HashPassword(u, u.Password);
                u.Password = hash;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", u.UserId);
                return Redirect("/home");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser lu)
        {
            if(ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.LoginEmail);
                if(userInDB == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email or Password");
                    return View("Index");
                }
                var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Email or Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("/home");
            }
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            ViewBag.User = current;
            List<AnActivity> AllActivities = _context.Activities
                                            .Include(m => m.Planner)
                                            .Include(m => m.Guests)
                                            .ThenInclude(wp => wp.ActivityGoer)
                                            .Where( m => m.StartDate >= DateTime.Now )
                                            .OrderBy( m => m.StartDate )
                                            .ToList();
            return View("Home", AllActivities);
        }

        [HttpGet("activity/new")]
        public IActionResult NewActivity()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            return View("CreateActivity");
        }

        [HttpPost("activity/create")]
        public IActionResult CreateActivity(AnActivity newActivity)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if(ModelState.IsValid)
            {
                newActivity.UserId = current.UserId;
                _context.Activities.Add(newActivity);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            return View("CreateActivity");
        }

        [HttpGet("activity/{activityId}/delete")]
        public IActionResult DeleteActivity(int activityId)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            AnActivity remove = _context.Activities.FirstOrDefault( m => m.ActivityId == activityId );
            _context.Activities.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("activity/{activityId}/{status}")]
        public IActionResult ToggleParty(int activityId, string status)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if(status == "join")
            {
                ActivityParty newParty = new ActivityParty();
                newParty.UserId = current.UserId;
                newParty.ActivityId = activityId;
                _context.Parties.Add(newParty);
            }
            else if(status == "leave")
            {
                ActivityParty backout = _context.Parties.FirstOrDefault( w => w.UserId == current.UserId && w.ActivityId == activityId );
                _context.Parties.Remove(backout);
            }
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("activity/{activityId}")]
        public IActionResult ShowActivity(int activityId)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            ViewBag.User = current;
            AnActivity meeting = _context.Activities
                                    .Include( m => m.Guests )
                                    .ThenInclude( w => w.ActivityGoer )
                                    .Include( m => m.Planner )
                                    .FirstOrDefault( m => m.ActivityId == activityId );
            return View(meeting);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }










    }
}
