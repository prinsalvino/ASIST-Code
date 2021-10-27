using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain
{
    public class UserBase
    {
        [Key]
        [OpenApiProperty(Description = "gets or sets the user id")]
        public long UserId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the first name")]
        [Required]
        public string FirstName { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the last name")]
        [Required]
        public string LastName { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the email address")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the password")]
        [Required]
        public string Password { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the user roles")]
        public UserRoles UserRole { get; set; }
        
        public UserBase(){}
        public UserBase(long userId, string firstName, string lastName, string emailAddress,
            string password)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Password = password;
        }
    }
}