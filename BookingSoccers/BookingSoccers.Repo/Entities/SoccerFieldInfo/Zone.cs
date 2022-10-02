

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookingSoccers.Repo.Entities.SoccerFieldInfo
{

    public class Zone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("FieldId")]
        public SoccerField Field { get; set; }

        [Required]
        public int FieldId { get; set; }

        [ForeignKey("ZoneTypeId")]
        public ZoneType ZoneCate { get; set; }

        [Required]
        public byte ZoneTypeId { get; set; }

        [Required]
        public byte Number { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Area { get; set; }
    }
}

