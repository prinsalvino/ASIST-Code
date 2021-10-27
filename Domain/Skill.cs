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
        public string Name { get; set; }

        [OpenApiProperty(Description = "gets or sets the students performing the skill")]
        [JsonIgnore]
        public virtual ICollection<SkillStudent> Students { get; set; }
        
    }
}