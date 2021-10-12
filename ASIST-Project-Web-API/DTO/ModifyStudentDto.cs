using System;
using Domain;

namespace ASIST.DTO
{
    public class ModifyStudentDto
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public long OrganisationId { get; set; }
        

        public ModifyStudentDto(){ }
        
    }
}