using System;
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
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Controllers
{
    public class AuthenticationHttpTrigger
    {
        ILogger Logger { get; }
        private readonly ITokenService tokenService;


        public AuthenticationHttpTrigger(ILogger<AuthenticationHttpTrigger> logger,  ITokenService tokenService)
        {
            Logger = logger;
            this.tokenService = tokenService;
        }
        
        [Function(nameof(AuthenticationHttpTrigger.Login))]
        [OpenApiOperation(operationId: "Login", tags: new [] {"authentication"}, Summary = "Login a User", Description = "User logs in with an email and password", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(UserLogin), Required = true, Description = "UserLogin object that needs to verify the login details of the user")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(JWTResponse), Summary = "User logged in Successfully", Description = "User logged in Successfully and gets Json Web Token")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Invalid user email/password", Description = "Invalid user email/password")]
        public async Task<HttpResponseData> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "login")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(await new StreamReader(req.Body).ReadToEndAsync());

                var jwtResponse = tokenService.CreateToken(userLogin);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                
                if (jwtResponse.Result == null)
                {
                    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                    await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                        "Invalid email address/password"));
                    responseData.StatusCode = HttpStatusCode.NotFound;
                    return responseData; 
                } 
                await response.WriteAsJsonAsync(jwtResponse);

                return response;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                    e.Message));
                responseData.StatusCode = HttpStatusCode.BadRequest;
                return responseData;
            }
        }
    }
}