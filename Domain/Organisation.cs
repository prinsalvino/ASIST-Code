using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class Organisation
    {
        [OpenApiProperty(Description = "gets or sets the organisation id")]
        public long OrganisationId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the organisation name")]
        [Required]
        public string OrganisationName { get; set; }
        
        [JsonIgnore]
        [OpenApiProperty(Description = "gets or sets the students")]
        public virtual ICollection<Student> Students { get; set; }
        
        [JsonIgnore]
        [OpenApiProperty(Description = "gets or sets the coaches")]
        public virtual ICollection<Coach> Coaches { get; set; }

    }
}