using Domain;
using Newtonsoft.Json;

namespace ASIST.DTO
{
    public class CreateCoachDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public CreateCoachDto(){}

        public CreateCoachDto(string firstName, string lastName, string emailAddress, string password, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            PhoneNumber = phoneNumber;
        }
    }
}