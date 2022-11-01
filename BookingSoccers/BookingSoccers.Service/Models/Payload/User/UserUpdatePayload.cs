using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.User
{
    public class UserUpdatePayload
    {
        public byte RoleId { get; set; }

        [Required]
        [StringLength(45)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "Phone is required and must be properly formatted.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(45)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
