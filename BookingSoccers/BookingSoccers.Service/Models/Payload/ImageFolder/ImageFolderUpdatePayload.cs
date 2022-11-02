using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ImageFolder
{
    public class ImageFolderUpdatePayload
    {
        public int FieldId { get; set; }

        [StringLength(200, ErrorMessage = "Path must contain at least 1 character and less than 200 character", MinimumLength = 1)]
        public string Path { get; set; }
    }
}
