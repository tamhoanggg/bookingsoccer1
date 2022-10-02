

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookingSoccers.Repo.Entities.SoccerFieldInfo
{

    public class PriceItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("PriceMenuId")]
        public PriceMenu Menu { get; set; }

        [Required]
        public int PriceMenuId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public byte TimeAmount { get; set; }
    }
}

