using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace BookingSoccers.Models
{
    //[Table("SoccerField")]
    public class SoccerField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ManagerId")]
        public User user { get; set; }

        [Required]
        public int ManagerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TimeSpan OpenHour { get; set; }

        [Required]
        public TimeSpan CloseHour { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public int TotalReviews { get; set; }

        [Required]
        public int ReviewScoreSum { get; set; }

        [Required]
        public int BaseTimeInterval { get; set; }
    }
}

