using System;
using Domain;

namespace ASIST_Web_API.DTO
{
    public class ModifyStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public long? OrganisationId { get; set; }


        public ModifyStudentDto()
        {
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            Password = "";
            OrganisationId = null;
        }
        
    }
}