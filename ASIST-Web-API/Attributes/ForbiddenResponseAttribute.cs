using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ASIST_Web_API.Attributes
{
    public class ForbiddenResponseAttribute:OpenApiResponseWithBodyAttribute
    {
        public ForbiddenResponseAttribute() : base(HttpStatusCode.Forbidden, "text/plain", typeof(string))
        {
            this.Description = "User login does not permit access to this function.";
        }
    }
}