using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer;

namespace ASIST.Controllers
{
    public class AuthenticationHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IUserService _userService;
            
        public AuthenticationHttpTrigger(ILogger<AuthenticationHttpTrigger> logger, IUserService userService)
        {
            Logger = logger;
            _userService = userService;
        }
        
        [Function(nameof(AuthenticationHttpTrigger.Login))]
        [OpenApiOperation(operationId: "Login", tags: new [] {"authentication"}, Summary = "Login a User", Description = "User logins with an email and password", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(UserLogin), Required = true, Description = "UserLogin object that needs to verify login details")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(JWTResponse), Summary = "Json Web Token", Description = "After logging in, you get a JWT response token")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Invalid user email/password", Description = "Invalid user email/password")]
        public async Task<HttpResponseData> Login(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "login")] HttpRequestData req,
            FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(requestBody);

            var jwtResponse = _userService.Login(userLogin);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            if (jwtResponse == null)
            {
                response = req.CreateResponse(HttpStatusCode.NotFound);
                return response; 
            } 
            
            await response.WriteAsJsonAsync(jwtResponse);

            return response;
        }
    }
}