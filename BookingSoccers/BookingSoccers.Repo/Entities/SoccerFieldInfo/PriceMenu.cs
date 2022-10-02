


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSoccers.Repo.Entities.SoccerFieldInfo
{
    public enum DayTypeEnum
    {
        Weekdays = 1,
        Weekends = 2,
        Holidays = 3
    }
    public class PriceMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("FieldId")]
        public SoccerField Field { get; set; }

        [Required]
        public int FieldId { get; set; }

        [ForeignKey("ZoneTypeId")]
        public ZoneType TypeOfZone { get; set; }

        [Required]
        public byte ZoneTypeId { get; set; }

        [Required]
        public DayTypeEnum DayType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public byte Status { get; set; }
    }
}

