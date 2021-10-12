using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class UserLogin
    {
        [JsonRequired]
        [OpenApiProperty(Description = "gets or sets the email address")]
        public string EmailAddress { get; set; }
        
        [JsonRequired]
        [OpenApiProperty(Description = "gets or sets the password")]
        public string Password { get; set; }
    }
}