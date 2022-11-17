using Web2ProjectMVC.Entities;
using Web2ProjectMVC.Repository;

namespace Web2ProjectMVC.ViewModels.Ads
{
    public class IndexVM
    {
        public List<Ad> Items { get; set; }
        public List<UserToAd> UsersToAds{ get; set; }
        public User LoggedUser { get; set; }

    }
}
