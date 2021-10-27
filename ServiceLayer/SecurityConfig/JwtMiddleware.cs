using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServiceLayer;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer.SecurityConfig
{
    public class JwtMiddleware : IFunctionsWorkerMiddleware
    {
        ITokenService TokenService { get; }
        ILogger Logger { get; }
        public JwtMiddleware(ITokenService tokenService, ILogger<JwtMiddleware> logger)
        {
            TokenService = tokenService;
            Logger = logger;
        }
     
        public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next)
        {
            string HeadersString = (string)Context.BindingContext.BindingData["Headers"];

            Dictionary<string, string> Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(HeadersString);

            if (Headers.TryGetValue("Authorization", out string AuthorizationHeader))
            {
                try
                {
                    AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(AuthorizationHeader);

                    ClaimsPrincipal User = await TokenService.GetByValue(BearerHeader.Parameter);

                    Context.Items.Add("User",User);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                }
            }

            await Next(Context);
        }
    }
}