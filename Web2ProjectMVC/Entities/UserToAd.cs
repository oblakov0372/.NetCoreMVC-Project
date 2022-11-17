using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2ProjectMVC.Entities
{
    public class UserToAd
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AdId { get; set; }
        public Status Status { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; }

    }

    public enum Status
    {
        NotWieved,
        Rejected,
        Accepted
    }
}
