using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class Student:UserBase
    {
               
        [OpenApiProperty(Description = "gets or sets the age of the student")]
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the sex of the student")]
        [Required]
        public Gender Gender { get; set; }

        [OpenApiProperty(Description = "gets or sets the skills performed")]
        [JsonIgnore]
        public virtual ICollection<SkillStudent> SkillsPerformed { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the tests attempted")]
        [JsonIgnore]
        public virtual ICollection<TestAttempt> TestsAttempted { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the sports for a student")]
        [JsonIgnore]
        public virtual ICollection<SportStudent> SportAdvices { get; set; }

        [OpenApiProperty(Description = "gets or sets the organisation id")]
        public long? OrganisationId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the organisation")]
        public Organisation Organisation { get; set; }

        public Student()
        {
            this.UserRole = UserRoles.Student;
        }
        public Student(long userId, string username, string firstName, string lastName, string emailAddress,
            string password, DateTime dateOfBirth, Gender gender)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.UserRole = UserRoles.Student;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
        }
    }
}