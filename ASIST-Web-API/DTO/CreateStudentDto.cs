using System;
using System.ComponentModel.DataAnnotations;
using Domain;
using Newtonsoft.Json;

namespace ASIST_Web_API.DTO
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "First name is mandatory")]
        [StringLength(250, MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is mandatory")]
        [StringLength(250, MinimumLength = 2)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "email address is mandatory")]
        [EmailAddress(ErrorMessage = "Incorrect email type")]
        [StringLength(250)]
        public string EmailAddress { get; set; }
        
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Date of birth is mandatory")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        //making the datetime and gender nullable otherwise they would have a preset value which would not run the required attribute
        public DateTime? DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Gender is mandatory, 0 is for male, 1 is for female")]
        public Gender? Gender { get; set; }
        
        [JsonIgnore]
        public UserRoles UserRoles { get; set; }
        
        
        public long? OrganisationId { get; set; }
        public CreateStudentDto(){}

    }
}