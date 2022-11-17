using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2ProjectMVC.Entities
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; }
    }
}
