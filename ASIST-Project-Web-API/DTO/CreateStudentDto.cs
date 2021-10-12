using System;
using Domain;
using Newtonsoft.Json;

namespace ASIST.DTO
{
    public class CreateStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [JsonIgnore]
        public UserRoles UserRoles { get; set; }
        
        public long OrganisationId { get; set; }
        public CreateStudentDto(){}

    }
}