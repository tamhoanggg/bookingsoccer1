
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSoccers.Models
{
    enum PaymentTypeEnum
    {
        Deposit=1, 
        PostPay=2
    }
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Booking BookingInfo { get; set; }

        [Required]
        public int BookingId { get; set; }

        public User ReceiverInfo { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}
