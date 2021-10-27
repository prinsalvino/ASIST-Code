using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Domain
{
    public class TestAttempt
    {
        [Key]
        [OpenApiProperty(Description = "gets or sets the test attempted id")]
        public long TestAttemptId { get; set; }

        public long StudentId { get; set; }
        [ForeignKey("StudentId")]
        [OpenApiProperty(Description = "gets or sets the student")]
        public virtual Student Student { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the student")]
        public int FinalScore { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the date of test")]
        public DateTime DateOfTest { get; set; }
    }
}