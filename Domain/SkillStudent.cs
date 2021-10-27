using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Domain
{
    public class SkillStudent
    {
        [Key]
        [OpenApiProperty(Description = "gets or sets the student and skill id")]
        public long SkillStudentId { get; set; }
        
        
        public long StudentId { get; set; }
        [ForeignKey("StudentId")]
        [OpenApiProperty(Description = "gets or sets the student")]
      
        public virtual Student Student { get; set; }
        
        public long? SkillId { get; set; }
        [ForeignKey("SkillId")]
        [OpenApiProperty(Description = "gets or sets the skill")]
       
        public virtual Skill Skill { get; set; }
        
        public long? CoachId { get; set; }
        [ForeignKey("CoachId")]
        [OpenApiProperty(Description = "gets or sets the coach")]
        public virtual Coach Coach { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the time of completion")]  
        [Required]
        public long TimeOfCompletion { get; set; }

        [OpenApiProperty(Description = "gets or sets the time of completion")]
        [Required]
        public DateTime DateOfSkill { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the score")]
        public int Score { get; set; }
    }
}