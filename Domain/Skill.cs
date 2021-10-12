using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Domain
{
    public class Skill
    {
        [OpenApiProperty(Description = "gets or sets the skill id")]
        public long SkillId { get; set; }

        [OpenApiProperty(Description = "gets or sets the type of skill")]
        public SkillType Type { get; set; }

        [OpenApiProperty(Description = "gets or sets the students performing the skill")]
        [JsonIgnore]
        public virtual ICollection<SkillStudent> Students { get; set; }
        
        //[OpenApiProperty(Description = "gets or sets the coaches affiliated with the student and skill")]
        //[JsonIgnore]
        //public virtual ICollection<Coach> Coaches { get; set; }

        //[OpenApiProperty(Description = "gets or sets the time of completion for each skill")]
       // public float TimeOfCompletion { get; set; }
        
       // [OpenApiProperty(Description = "gets or sets the score of skill")]
       // public int Score { get; set; }
    }
}