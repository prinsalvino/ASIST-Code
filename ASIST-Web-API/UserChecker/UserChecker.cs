using ASIST_Project_Web_API.Utils;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASIST_Project_Web_API.UserChecker
{
    public class UserChecker
    {
		ILogger logger;
        public UserChecker(ILogger logger)
        {
          this.logger = logger;
		
		}
		public async Task<HttpResponseData> ExecuteForUser(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();
				// Get Current User Email using
				// object name = User.Identities.FirstOrDefault().Name;
				// Get UserId
				//var claims = User.Identities.FirstOrDefault().Claims.ToList();
				//long userId = Convert.ToInt64(claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault());

				try
				{
					return await Delegate(User).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.BadRequest);
					logger.LogError(e, e.ToString());
					return Response;
				}
			}
			catch (Exception e)
			{
				logger.LogError(e.Message);

				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}

		public async Task<HttpResponseData> ExecuteForStudent(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();

				if (User.IsInRole(UserRoles.Student.ToString()) || User.IsInRole(UserRoles.Admin.ToString()))
				{
					try
					{
						return await Delegate(User).ConfigureAwait(false);
					}
					catch (Exception e)
					{
						HttpResponseData response = Request.CreateResponse(HttpStatusCode.BadRequest);
						return response;
					}
				}
				else
				{
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);

					return Response;
				}

			}
			catch (Exception e)
			{
				logger.LogError(e.Message);

				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}

		public async Task<HttpResponseData> ExecuteCoach(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();

				if (User.IsInRole(UserRoles.Coach.ToString()) || User.IsInRole(UserRoles.Admin.ToString()))
				{
					try
					{
						
						return await Delegate(User).ConfigureAwait(false);
					}
					catch (Exception e)
					{
						HttpResponseData response = Request.CreateResponse(HttpStatusCode.BadRequest);
						return response;
					}
				}
                else
                {
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);

					return Response;
				}
				
			}
			catch (Exception e)
			{
				logger.LogError(e.Message);

				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}

		public async Task<HttpResponseData> ExecuteAdmin(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();

				if (User.IsInRole(UserRoles.Admin.ToString()))
				{
					try
					{
						return await Delegate(User).ConfigureAwait(false);
					}
					catch (Exception e)
					{
						HttpResponseData response = Request.CreateResponse(HttpStatusCode.BadRequest);
						return response;
					}
				}
				else
				{
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);

					return Response;
				}

			}
			catch (Exception e)
			{
				logger.LogError(e.Message);

				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}


	}
}
