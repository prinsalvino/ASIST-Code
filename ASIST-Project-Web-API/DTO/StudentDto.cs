using System;
using Domain;
using Newtonsoft.Json;

namespace ASIST.DTO
{
    public class StudentDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public long OrganisationId { get; set; }
        
        public StudentDto()
        {
            
        }
    }
}