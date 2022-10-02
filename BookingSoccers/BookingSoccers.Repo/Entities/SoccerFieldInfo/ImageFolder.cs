
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookingSoccers.Repo.Entities.SoccerFieldInfo
{

    public class ImageFolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("FieldId")]
        public SoccerField Field { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        [StringLength(200)]
        public string Path { get; set; }

    }
}

