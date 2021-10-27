using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class Coach : UserBase
    {
        [OpenApiProperty(Description = "Gets or sets the phone number")]
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public virtual ICollection<SkillStudent> SkillsAssessed { get; set; }

        [JsonIgnore]
        private ICollection<Organisation> organisations;
        [JsonIgnore]
        public virtual ICollection<Organisation> Organisations
        {
            get { return organisations ?? (organisations = new Collection<Organisation>()); }
            set { organisations = value; }
        }
        public Coach()
        {
            this.UserRole = UserRoles.Coach;
        }
        public Coach(long userId, string firstName, string lastName, string emailAddress,
            string password)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.UserRole = UserRoles.Coach;
        }

        public Coach(long userId, string firstName, string lastName, string emailAddress,
            string password, string phoneNumber)

        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.UserRole = UserRoles.Coach;
            this.PhoneNumber = phoneNumber;
        }
    }
}