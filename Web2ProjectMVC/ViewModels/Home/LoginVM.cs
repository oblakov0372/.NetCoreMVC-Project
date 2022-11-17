using System.ComponentModel.DataAnnotations;

namespace Web2ProjectMVC.ViewModels.Home
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
