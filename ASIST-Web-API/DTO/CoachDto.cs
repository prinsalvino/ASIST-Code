using System.Collections.Generic;
using Domain;

namespace ASIST_Web_API.DTO
{
    public class CoachDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }


        public CoachDto(){}
    }
}