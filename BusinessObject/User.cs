using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace BusinessObject
{
    public class User : IdentityUser
    {
        [RegularExpression(@"^\S+$", ErrorMessage = "User Name cannot contain whitespace.")]
        public override string? UserName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Email cannot contain whitespace.")]
        public override string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\p{L}\p{M}]+(?:\s[a-zA-Z\p{L}\p{M}]+)*$", ErrorMessage = "Full Name cannot contain special characters.")]
        public string? FullName { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDay { get; set; }
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone Number must contain 10 digits, start with 0, and not contain special characters or letters.")]
        public override string? PhoneNumber { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
        public string? Role { get; set; }
        public string? PhotoUrl { get; set; }
        public List<Gift>? Gifts { get; set; }
    }
}