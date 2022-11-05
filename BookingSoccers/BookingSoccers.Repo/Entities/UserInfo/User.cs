

using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookingSoccers.Repo.Entities.UserInfo
{
    public enum GenderEnum
    {
        Male = 1,
        Female = 2
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role role { get; set; }

        [Required]
        public byte RoleId { get; set; }

        public ICollection<SoccerField> SoccerFields { get; set; }  

        public ICollection<Booking> Bookings { get; set; }

        [Required]
        [StringLength(45)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(45)]
        public string Email { get; set; }
    }
}

