using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    [NotMapped]
    public class CoachOrganisation
    {
        [OpenApiProperty(Description = "gets or sets the coach")]
        public virtual Coach Coach { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the organisation")]
        public virtual Organisation Organisation { get; set; }

        public CoachOrganisation()
        {
            
        }
    }
}