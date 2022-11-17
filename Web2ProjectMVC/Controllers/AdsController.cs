using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web2ProjectMVC.ActionFilter;
using Web2ProjectMVC.Entities;
using Web2ProjectMVC.ExtentionMethods;
using Web2ProjectMVC.Repository;
using Web2ProjectMVC.Services;
using Web2ProjectMVC.ViewModels.Ads;

namespace Web2ProjectMVC.Controllers
{
    public class AdsController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly AdsService _adsService;
        public AdsController(ProjectDbContext context,AdsService adsService)
        {
            _context = context;
            _adsService = adsService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IndexVM model = new IndexVM();
            model.Items = _context.Ads.ToList();
            model.UsersToAds = _context.UserStatuses.ToList();
            model.LoggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            return View(model);
        }
        [AdsFilter]
        [HttpGet]
        public IActionResult OurAds()
        {
            User organization = HttpContext.Session.GetObject<User>("loggedUser");
            if (organization.Role == Roles.User)
                return RedirectToAction("Index", "Home");
            IndexVM model = new IndexVM();
            model.Items = _context.Ads.Where(ad => ad.OwnerId == organization.Id).ToList();
            model.UsersToAds = _context.UserStatuses.ToList();
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            _adsService.Delete(id);

            return RedirectToAction("OurAds", "Ads");
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateVM model = new CreateVM();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            User organization = HttpContext.Session.GetObject<User>("loggedUser");
            if (organization == null || organization.Role == Roles.User)
                return RedirectToAction("Index", "Home");
            Ad newAd = new Ad();
            newAd.Title = model.Title;
            newAd.Description = model.Description;
            newAd.OwnerId = organization.Id;
            _context.Ads.Add(newAd);
            _context.SaveChanges();
            return RedirectToAction("OurAds", "Ads");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Ad item = _context.Ads.Where(ad => ad.Id == id).FirstOrDefault();
            if (item == null)
                return RedirectToAction("OurAds", "Ads");

            EditVM model = new EditVM();

            model.Id = item.Id;
            model.Title = item.Title;
            model.Description = item.Description;

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User organization = HttpContext.Session.GetObject<User>("loggedUser");
            if (organization == null || organization.Role == Roles.User)
                return RedirectToAction("Index", "Home");

            Ad item = new Ad
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                OwnerId = organization.Id,
            };
            _context.Ads.Update(item);
            _context.SaveChanges();
            return RedirectToAction("OurAds", "Ads");
        }
        
    }
}
