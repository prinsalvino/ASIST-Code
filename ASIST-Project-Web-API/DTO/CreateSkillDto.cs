using System;
using Newtonsoft.Json;

namespace ASIST.DTO
{
    public class CreateSkillDto
    {
        [JsonIgnore]
        public long StudentId { get; set; }
        public long SkillId { get; set; }
        public long CoachId { get; set; }
        
        
        public string Time { get; set; }
        
        [JsonIgnore]
        public TimeSpan TimeTaken { get; set; }
        
        [JsonIgnore]
        public long TimeOfCompletion { get; set; }
        public DateTime DateOfSkill { get; set; }

        public CreateSkillDto()
        {
            this.DateOfSkill = DateTime.Now;
        }
    }
}