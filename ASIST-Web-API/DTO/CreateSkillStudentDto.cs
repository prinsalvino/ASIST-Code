using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASIST_Web_API.DTO
{
    public class CreateSkillStudentDto
    {
        [JsonIgnore]
        public long StudentId { get; set; }
        
        [Required(ErrorMessage = "Please enter a skill")]
        public long SkillId { get; set; }
        
        [Required(ErrorMessage = "Please enter a coach id")]
        public long CoachId { get; set; }
        
        [Required(ErrorMessage = "Please enter the time taken for this skill")]
        [DataType(DataType.Time)]
        public string Time { get; set; }
        
        [JsonIgnore]
        public TimeSpan TimeTaken { get; set; }
        
        [JsonIgnore]
        public long TimeOfCompletion { get; set; }
        
        [JsonIgnore]
        public DateTime DateOfSkill { get; set; }

        public CreateSkillStudentDto()
        {
            this.DateOfSkill = DateTime.Now;
        }
    }
}