using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ImageFolder
{
    public class ImageListCreateForm
    {
        [RegularExpression("^[0-9]+$", ErrorMessage = "FieldId is an Positive Integer.")]
        public int FieldId { get; set; }

        public string AccessToken { get; set; }
    }
}
