using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Validations
{
    public class GenderValidation
    {
        public sealed class CheckGender : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if(Enum.IsDefined( typeof(GenderEnum), value)) 
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult
                    ("Invalid Gender "+ value + ", valid genders are: Male or Female");

            }
        }
    }
}
