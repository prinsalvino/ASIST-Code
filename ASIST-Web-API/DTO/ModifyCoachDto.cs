namespace ASIST_Web_API.DTO
{
    public class ModifyCoachDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }


        public ModifyCoachDto()
        {
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            Password = "";
            PhoneNumber = "";
        }
    }
}