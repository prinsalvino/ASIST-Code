using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Domain
{
    public class SportStudent
    {
        [OpenApiProperty(Description = "gets or sets the student and sport id")]
        public long SportStudentId { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the student")]
        public virtual Student Student { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the sport")]
        public virtual Sport Sport { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the date for the sport advices")]
        public DateTime DateOfSportAdvices { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the score of skill")]
        public int Score { get; set; }
    }
}