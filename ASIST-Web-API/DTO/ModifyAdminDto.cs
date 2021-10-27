using System.ComponentModel.DataAnnotations;

namespace ASIST_Web_API.DTO
{
    public class ModifyAdminDto
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public ModifyAdminDto()
        {
            //initializing with empty strings because it complains of null values when there is no value in the property
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            Password = "";
        }
    }
}