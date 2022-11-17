using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Web2ProjectMVC.ViewModels.Ads
{
    public class EditVM
    {
        public int Id { get; set; }
        [DisplayName("Title : ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Title { get; set; }
        [DisplayName("Description : ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Description { get; set; }
    }
}
