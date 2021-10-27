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
    public class SkillHttpTrigger
    {
        ILogger Logger;
        private readonly ISkillService _skillService;
        private readonly IMapper _mapper;
        
        UserChecker userChecker;
        public SkillHttpTrigger(ILogger<SkillHttpTrigger> logger, IMapper mapper, ISkillService skillService)
        {
            Logger = logger;
            userChecker = new UserChecker(logger);
            _skillService = skillService;
            _mapper = mapper;
        }
        
        [Function(nameof(SkillHttpTrigger.GetSkills))]
        [OpenApiOperation(operationId: "GetSkills", tags: new[] {"StudentOperations", "CoachOperations", "Skill" }, Summary = "Get skills", Description = "Getting a list of skills from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Skill>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "no skills found", Description = "no skills found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSkills(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "skills")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var skills = _skillService.GetAllSkills();
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<Skill>>(skills));
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
        
        [Function(nameof(SkillHttpTrigger.GetSkillById))]
        [OpenApiOperation(operationId: "GetSkillById", tags: new[] {"StudentOperations", "CoachOperations", "Skill" }, Summary = "Get skill By skill id", Description = "Getting a skill using skill id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "skillId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of skill", Description = "Id of skill", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SkillDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid skill id supplied", Description = "Invalid skill id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "skill not found", Description = "skill not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSkillById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "skills/{skillId}")] HttpRequestData req,
            FunctionContext executionContext, long skillId)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var skill = _skillService.GetSkillById(skillId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<SkillDto>(skill));
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