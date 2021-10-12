using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class Organisation
    {
        [OpenApiProperty(Description = "gets or sets the organisation id")]
        public long OrganisationId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the organisation name")]
        public string OrganisationName { get; set; }
        
        [JsonIgnore]
        [OpenApiProperty(Description = "gets or sets the students")]
        public virtual ICollection<Student> Students { get; set; }

        [OpenApiProperty(Description = "gets or sets the coach id for that organisation")]
        public long? CoachId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the coach for that organisation")]
        public Coach Coach { get; set; }
    }
}