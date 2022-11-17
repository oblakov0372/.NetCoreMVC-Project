using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;
using Web2ProjectMVC.ActionFilter;
using Web2ProjectMVC.Entities;
using Web2ProjectMVC.ExtentionMethods;
using Web2ProjectMVC.Repository;
using Web2ProjectMVC.Services;
using Web2ProjectMVC.ViewModels.Home;

namespace Web2ProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly AdsService _adsService;
        public HomeController(ProjectDbContext context,AdsService adsService)
        {
            _context = context;
            _adsService = adsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if(!this.ModelState.IsValid)
                return View(model);

            User loggedUser = _context.Users.Where(u => u.Password == model.Password && u.UserName == model.UserName)
                                            .FirstOrDefault(); 
            if(loggedUser == null)
            {
                this.ModelState.AddModelError("authError", "Invalid Username or password");
                return View(model);
            }
            HttpContext.Session.SetObject<User>("loggedUser",loggedUser);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            User item = _context.Users.Where(u => u.UserName == model.UserName || u.Email == model.Email)
                                      .FirstOrDefault();
            
            if(item != null)
            {
                this.ModelState.AddModelError("alreadyRegistered", "User with this Email or Username already registered");
                return View(model);
            }
            User newItem = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Created = DateTime.Now,
                Role = Roles.User
            };
            _context.Users.Add(newItem);
            _context.SaveChanges();
            return RedirectToAction("Login", "Home");
        }
        
        [AuthenticationFilter]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.SetObject<User>("loggedUser", null);
            return RedirectToAction("Login", "Home");
        }
    }
}
