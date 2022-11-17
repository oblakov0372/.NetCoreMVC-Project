using System.ComponentModel.DataAnnotations;
using Web2ProjectMVC.Entities;

namespace Web2ProjectMVC.ViewModels.Users
{
    public class EditVM
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}
