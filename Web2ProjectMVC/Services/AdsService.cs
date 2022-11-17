using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web2ProjectMVC.Entities;
using Web2ProjectMVC.ExtentionMethods;
using Web2ProjectMVC.Repository;
using Web2ProjectMVC.ViewModels.Ads;

namespace Web2ProjectMVC.Services
{
    public class AdsService
    {
        private readonly ProjectDbContext _context;
        public AdsService(ProjectDbContext context)
        {
            _context = context;
        }
        public bool Delete(int id)
        {
            Ad adForDelete = _context.Ads.Where(ad => ad.Id == id).FirstOrDefault();
            if (adForDelete == null)
                return false;

            List<UserToAd> items = _context.UserStatuses.Where(i => i.AdId == id).ToList();
            foreach (UserToAd item in items)
            {
                _context.UserStatuses.Remove(item);
            }

            _context.Ads.Remove(adForDelete);
            _context.SaveChanges();

            return true;
        }
        
    }
}
