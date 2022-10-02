
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.Entities.UserInfo;

namespace BookingSoccers.Repo.Entities.BookingInfo
{

    public enum StatusEnum
    {
        Waiting = 1,
        CheckedIn = 2,
        CheckedOut = 3,
        Canceled = 4
    }

    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public User Customer { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("ZoneTypeId")]
        public ZoneType TypeZone { get; set; }

        [Required]
        public byte ZoneTypeId { get; set; }

        [ForeignKey("ZoneId")]
        public Zone ZoneInfo { get; set; }

        [Required]
        public int ZoneId { get; set; }

        [ForeignKey("FieldId")]
        public SoccerField FieldInfo { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        public byte Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }
    }

}
