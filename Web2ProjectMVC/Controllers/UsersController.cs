using Microsoft.AspNetCore.Mvc;
using Web2ProjectMVC.ActionFilter;
using Web2ProjectMVC.Entities;
using Web2ProjectMVC.ExtentionMethods;
using Web2ProjectMVC.Repository;
using Web2ProjectMVC.Services;
using Web2ProjectMVC.ViewModels.Users;

namespace Web2ProjectMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly AdsService _adsService;
        public UsersController(ProjectDbContext context,AdsService adsService)
        {
            _context = context;
            _adsService = adsService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IndexVM model = new IndexVM();
            model.Items = _context.Users.Where(u => u.Role != Roles.Admin).ToList();

            return View(model);
        }
        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User itemForEdit = _context.Users.Where(i => i.Id == id).FirstOrDefault();
            if (itemForEdit == null)
                return RedirectToAction("Profile", "Users");


            EditVM model = new EditVM();
            model.Id = itemForEdit.Id;
            model.UserName = itemForEdit.UserName;
            model.Password = itemForEdit.Password;
            model.Email = itemForEdit.Email;
            model.FirstName = itemForEdit.FirstName;
            model.LastName = itemForEdit.LastName;
            model.Role = itemForEdit.Role;

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            //User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            if (!ModelState.IsValid)
                return View(model);

            //User checkItem = _context.Users.Where(u => u.UserName == model.UserName || u.Email == model.Email)
            //                          .FirstOrDefault();

            //if (checkItem != null && (loggedUser.UserName != model.UserName || loggedUser.Email != model.Email))
            //{
            //    this.ModelState.AddModelError("alreadyRegistered", "User with this Email or Username already registered");
            //    return View(model);
            //}

            User item = new User();
            item.Id = model.Id;
            item.UserName = model.UserName;
            item.Password = model.Password;
            item.Email = model.Email;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Role = model.Role;
            item.Updated = DateTime.Now;
            
            _context.Users.Update(item);
            _context.SaveChanges();
            return RedirectToAction("Profile", "Users");
        }

        [HttpPost]
        public IActionResult ToOrganization(int id)
        {
            User user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Users");

            user.Role = Roles.Organization;
            user.Updated = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();


            return RedirectToAction("Index","Users");
        }
        [HttpPost]
        public IActionResult ToUser(int id)
        {
            User user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Users");
            List<Ad> ads = _context.Ads.Where(ad => ad.OwnerId == id).ToList();
            if (ads.Count != 0)
            {
                foreach(Ad ad in ads)
                {
                    _adsService.Delete(ad.Id);
                }
            }
            user.Role = Roles.User;
            user.Updated = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Users");
        }
        [AuthenticationFilter]
        public IActionResult EngageUser(int id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            UserToAd item = _context.UserStatuses.Where(i => i.UserId == loggedUser.Id && i.AdId == id).FirstOrDefault();

            if (item != null)
                return RedirectToAction("Index", "Ads");

            item = new UserToAd();

            item.AdId = id;
            item.UserId = loggedUser.Id;
            item.Status = Status.NotWieved;

            _context.UserStatuses.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index", "Ads");
        }
        public IActionResult ConfirmUser(int idAd,int idUser)
        {
            UserToAd userToAd = _context.UserStatuses.Where(i => i.UserId == idUser && i.AdId == idAd).FirstOrDefault();
            if (userToAd == null)
                return RedirectToAction("OurAds", "Ads");

            userToAd.Status = Status.Accepted;
            _context.UserStatuses.Update(userToAd);
            _context.SaveChanges();

            return RedirectToAction("OurAds", "Ads");
        }

        public IActionResult RejectUser(int idAd, int idUser)
        {
            UserToAd userToAd = _context.UserStatuses.Where(i => i.UserId == idUser && i.AdId == idAd).FirstOrDefault();
            if (userToAd == null)
                return RedirectToAction("OurAds", "Ads");

            userToAd.Status = Status.Rejected;
            _context.UserStatuses.Update(userToAd);
            _context.SaveChanges();

            return RedirectToAction("OurAds", "Ads");
        }

        [AuthenticationFilter]
        [HttpGet]
        public IActionResult Delete()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            if (loggedUser == null)
                return RedirectToAction("Index", "Home");

            if (loggedUser.Role == Roles.User)
            {
                List<UserToAd> adsWhereCandidate = _context.UserStatuses.Where(item => item.UserId == loggedUser.Id).ToList();
                foreach (var item in adsWhereCandidate)
                {
                    _context.UserStatuses.Remove(item);
                }
            }
            else if (loggedUser.Role == Roles.Organization)
            {
                List<Ad> ads = _context.Ads.Where(item => item.OwnerId == loggedUser.Id).ToList();
                foreach (var item in ads)
                {
                    _adsService.Delete(item.Id);
                }
            }
            _context.Users.Remove(loggedUser);

            _context.SaveChanges();

            return RedirectToAction("Login", "Home");
        }
    }
}
