using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace ASIST_Web_API.Attributes
{
    public class AsistAuthAttribute:OpenApiSecurityAttribute
    {
        public AsistAuthAttribute() : base("AsistAuth", SecuritySchemeType.Http)
        {
            Description = "JWT for authorization";
            In = OpenApiSecurityLocationType.Header;
            Scheme = OpenApiSecuritySchemeType.Bearer;
            BearerFormat = "JWT";
        }
    }
}