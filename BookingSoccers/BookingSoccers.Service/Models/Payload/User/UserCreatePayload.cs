using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingSoccers.Repo.Validations.GenderValidation;

namespace BookingSoccers.Service.Models.Payload.User
{
    public class UserCreatePayload
    {
        public byte RoleId { get; set; }

        [Required(ErrorMessage = "Username cannot be null or empty")]
        [StringLength(0, ErrorMessage = "UserName must contain at least 1 character and less than 45 character", MinimumLength = 45)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName cannot be null or empty")]
        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 20 character", MinimumLength = 20)]
        public string FirstName { get; set; }

        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 20 character", MinimumLength = 20)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender cannot be null or empty")]
        [CheckGender]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "Phone cannot be null or empty")]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "Phone is required and must be properly formatted.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email cannot be null or empty")]
        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 45 character", MinimumLength = 45)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
