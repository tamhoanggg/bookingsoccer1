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
        [Required(ErrorMessage = "RoleID cannot be null or empty")]
        [Range(1, 3, ErrorMessage = "Role ID between 1 to 3.")]
        public byte RoleId { get; set; }

        [Required(ErrorMessage = "Username cannot be null or empty")]
        [StringLength(45, ErrorMessage = "UserName must contain at least 1 character and less than 45 character", MinimumLength = 1)]
        public string UserName { get; set; }


        [Required(ErrorMessage = "FirstName cannot be null or empty")]
        [StringLength(20, ErrorMessage = "First Name must contain at least 1 character and less than 20 character", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName cannot be null or empty")]
        [StringLength(20, ErrorMessage = "Last Name must contain at least 1 character and less than 20 character", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender cannot be null or empty")]
        [CheckGender]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "Phone cannot be null or empty")]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "Phone is required and must be properly formatted.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email cannot be null or empty")]
        [StringLength(45, ErrorMessage = "Name must contain at least 1 character and less than 45 character", MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
