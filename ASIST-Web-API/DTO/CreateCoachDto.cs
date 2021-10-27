using System.ComponentModel.DataAnnotations;
using Domain;
using Newtonsoft.Json;

namespace ASIST_Web_API.DTO
{
    public class CreateCoachDto
    {
        [Required(ErrorMessage = "First name is mandatory")]
        [StringLength(250, MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "last name is mandatory")]
        [StringLength(250, MinimumLength = 2)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "email address is mandatory")]
        [StringLength(250)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }
        
        [Required(ErrorMessage = "password is mandatory")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "phone number is mandatory")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }
}