using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Admin : UserBase
    {
        public Admin()
        {
            this.UserRole = UserRoles.Admin;
        }
        public Admin(long userId, string firstName, string lastName, string emailAddress,
            string password)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.UserRole = UserRoles.Admin;
        }
    }
}