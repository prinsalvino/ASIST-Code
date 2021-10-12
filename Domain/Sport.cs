using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domain
{
    public class Sport
    {
        [OpenApiProperty(Description = "gets or sets the sport id")]
        public long SportId { get; set; }

        [OpenApiProperty(Description = "gets or sets the name of sport")]
        public string Name { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the sports advices")]
        [JsonIgnore]
        public virtual ICollection<SportStudent> SportAdvices { get; set; }

        //[OpenApiProperty(Description = "gets or sets the score of skill")]
        //public int Score { get; set; }
    }
}
