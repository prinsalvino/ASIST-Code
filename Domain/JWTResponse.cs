using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Domain
{
    public class JWTResponse
    {
        [OpenApiProperty(Description = "gets or sets the jwt token")]
        private string Token { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the type of the token")]
        private string Type { get; set; }

        [OpenApiProperty(Description = "gets or sets the Id")]
        private long Id { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the email")]
        private string Email { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the user role")]
        private UserRoles UserRole { get; set; }

        public JWTResponse(UserBase user, string token)
        {
            Token = token;
            Type = "bearer";
            Id = user.UserId;
            Email = user.EmailAddress;
            UserRole = user.UserRole;
        }
    }
}