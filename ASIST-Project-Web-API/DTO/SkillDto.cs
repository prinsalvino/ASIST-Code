using System;
using Newtonsoft.Json;

namespace ASIST.DTO
{
    public class SkillDto
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