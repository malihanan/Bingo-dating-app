using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bingo.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Password not strong enough.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$",
            ErrorMessage = "Contact is invalid.")]
        public string Contact { get; set; }
        public string Gender { get; set; }
        [Required]
        [ValidateDateRange]
        public DateTime Birthdate { get; set; }
        public bool DisplayBirthdate { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Bio { get; set; }
        public string Likes { get; set; }
        public string Dislikes { get; set; }
        public string Hobbies { get; set; }
    }
}
public class ValidateDateRange : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((DateTime)value <= DateTime.Now.AddYears(-18))
        {
            return ValidationResult.Success;
        }
        else
        {
            int year = 18 - (DateTime.Now.Year - ((DateTime)value).Year);
            return new ValidationResult("Come back after " + year + " years");
        }
    }
}