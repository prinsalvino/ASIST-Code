using System;
using Newtonsoft.Json;

namespace ASIST_Web_API.DTO
{
    public class SkillStudentDto
    {
        public long StudentId { get; set; }
        public long SkillId { get; set; }
        [JsonIgnore]
        public long TimeOfCompletion { get; set; }

        public TimeSpan TimeTaken { get {return TimeSpan.FromTicks(TimeOfCompletion);}}

        public DateTime DateOfSkill { get; set; }
        public int Score { get; set; }

      
    }
}