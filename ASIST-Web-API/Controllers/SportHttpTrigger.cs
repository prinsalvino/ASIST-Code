using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ASIST_Project_Web_API.UserChecker;
using ASIST_Web_API.Attributes;
using ASIST_Web_API.DTO;
using AutoMapper;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Controllers
{
    public class SportHttpTrigger
    {
        ILogger Logger;
        private readonly ISportService _sportService;
        private readonly IMapper _mapper;
        UserChecker userChecker;
        public SportHttpTrigger(ILogger<SportHttpTrigger> logger, IMapper mapper, ISportService sportService)
        {
            Logger = logger;
            userChecker = new UserChecker(logger);
            _sportService = sportService;
            _mapper = mapper;
        }
        
        [Function(nameof(SportHttpTrigger.GetSports))]
        [OpenApiOperation(operationId: "GetSports", tags: new[] {"StudentOperations", "CoachOperations", "Sport" }, Summary = "Get Sports", Description = "Getting a list of sports from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<SportDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "no Sports found", Description = "no Sports found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSports(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "sports")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var sports = _sportService.GetAllSports();
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<SportDto>>(sports));
                        return response;
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }
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
            });
        }
        
        [Function(nameof(SportHttpTrigger.GetSportsById))]
        [OpenApiOperation(operationId: "GetSportsById", tags: new[] {"StudentOperations", "CoachOperations", "Sport" }, Summary = "Get Sport By sport id", Description = "Getting a sport using sport id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "sportId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of sport to return", Description = "Id of sport to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SportDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid sport id supplied", Description = "Invalid sport id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Sport not found", Description = "Sport not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSportsById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "sports/{sportId}")] HttpRequestData req,
            FunctionContext executionContext, long sportId)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var sport = _sportService.GetSportById(sportId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<SportDto>(sport));
                        return response;
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }
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
            });
        }
    }
}