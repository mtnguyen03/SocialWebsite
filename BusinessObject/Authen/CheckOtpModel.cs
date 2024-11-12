using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Authen
{
    public class CheckOtpModel
    {

        [Required(ErrorMessage = "OTP is required.")]
        public string Otp { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
