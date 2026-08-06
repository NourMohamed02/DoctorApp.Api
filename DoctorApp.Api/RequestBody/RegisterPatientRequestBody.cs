using System.ComponentModel.DataAnnotations;

namespace DoctorApp.Api.RequestBody
{
    public class RegisterPatientRequestBody
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Password must be exactly 7 characters.")]
        public string password { get; set; }
    }
}