using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Domain
{
    public class JWTResponse
    {
        private JwtSecurityToken Token { get; }

        [OpenApiProperty(Description = "The access token to be used in every subsequent operation for this user.")]
        [JsonRequired]
        public string AccessToken => new JwtSecurityTokenHandler().WriteToken(Token);


        [OpenApiProperty(Description = "gets or sets the type of the token")]
        public string Type { get; set; }

        [OpenApiProperty(Description = "gets or sets the Id")]
        public long Id { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the email")]
        public string Email { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the user role")]
        public string UserRole { get; set; }

        public JWTResponse(UserBase user, JwtSecurityToken token)
        {
            this.Token = token;
            Type = "Bearer";
            Id = user.UserId;
            Email = user.EmailAddress;
            UserRole = user.UserRole.ToString();
        }
        public JWTResponse(JwtSecurityToken token)
        {
            this.Token = token;
        }
    }
}