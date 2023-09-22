using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunRose_Test.Models;
using SunRose_Test.Repository;
using System.Security.Claims;
using System.Web;

namespace SunRose_Test.Controllers
{
    public class AccountController : Controller
    {
        private readonly JsonRepository<User> JsonRepository;
        static private int nextId;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController()
        {
            JsonRepository = new JsonRepository<User>();
        }
        //public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    JsonRepository = new JsonRepository<User>();
        //}

        [HttpGet]
        public IActionResult LogIn()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult LogIn(User loginUser)
        {
            if (!ModelState.IsValid)
                return View(loginUser);

            var usersList = JsonRepository.Read();

            User user = usersList.Where(s => s.Username == loginUser.Username).FirstOrDefault();

            if (user != null)
            {
                if (user.Password == loginUser.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
                    };
                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));


                    return RedirectToAction("Post", "SunRose", new { userId = user.Id });
                }
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(loginUser);
        }


        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult SignUp()
        {
            User user = new User(nextId, "");
            return View(user);
        }

        
        [HttpPost]
        public ActionResult SignUp(User newUser)
        {
            if (ModelState.IsValid)
            {
                newUser.Feed = new Queue<Message>();

                var usersList = JsonRepository.Read();

                var user = usersList.Where(x => x.Id == newUser.Id).FirstOrDefault();

                if (user == null)
                    usersList.Add(newUser);
                else
                {
                    ModelState.AddModelError("", "User with this Id already exists");
                    return View();
                }

                nextId++;

                JsonRepository.Create(usersList);

                return RedirectToAction("Index", "Account");
            }
            else
                return View();

        }


        public ActionResult Index()
        {
            var usersList = JsonRepository.Read();
            return View(usersList);
        }
    }
}
