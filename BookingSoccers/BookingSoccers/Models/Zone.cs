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
    //[Table("Zone")]
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

