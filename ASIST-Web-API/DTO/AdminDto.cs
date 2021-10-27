using System.ComponentModel.DataAnnotations;

namespace ASIST_Web_API.DTO
{
    public class AdminDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public AdminDto(){}
    }
}