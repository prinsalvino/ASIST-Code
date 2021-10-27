using ASIST_Project_Web_API.UserChecker;
using ASIST_Web_API.Attributes;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASIST_Web_API.DTO;
using AutoMapper;
using ServiceLayer;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Controllers
{
    public class SportAdviceHttpTrigger
    {
        ILogger Logger;
        private readonly ISportStudentService _sportStudentService;
        private readonly ISportService _sportService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        UserChecker userChecker;
        private CurrentUserGetter _currentUserGetter;
        public SportAdviceHttpTrigger(ILogger<SportAdviceHttpTrigger> logger, ISportStudentService sportStudentService, IMapper mapper, IUserService userService, ISportService sportService)
        {
            Logger = logger;
            _sportService = sportService;
            _sportStudentService = sportStudentService;
            _userService = userService;
            userChecker = new UserChecker(logger);
            _mapper = mapper;
            _currentUserGetter = new CurrentUserGetter();
        }

        [Function(nameof(SportAdviceHttpTrigger.GetSportAdviceByStudentIdAndSportId))]
        [OpenApiOperation(operationId: "GetSportAdviceByStudentIdAndSportId", tags: new[] { "StudentOperations", "CoachOperations", "SportAdvice" }, Summary = "Get Sport Advice  By Sport Id and student id", Description = "Getting a sport advice using a sport and student id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "sportId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of sport", Description = "Id of sport", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SportAdviceDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid sport or student id supplied", Description = "Invalid sport or student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Sport Advice not found", Description = "Sport Advice not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSportAdviceByStudentIdAndSportId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "sportadvices/{studentId}/{sportId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId, long sportId)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        UserBase currentUser = _currentUserGetter.getCurrentUser(User, _userService);
                        //checking if current user is student
                        if (currentUser.UserRole == UserRoles.Student)
                        {
                            //if a student is logged in, they cannot view the details of another student
                            if (studentId != currentUser.UserId)
                            {
                                throw new Exception("Not allowed to view other student's sport advices");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.Forbidden);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.Forbidden;
                        return responseData;
                    }
                    
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        _sportService.GetSportById(sportId);
                        var sportAdvice = _sportStudentService.GetSportAdviceByStudentIdAndSportId(studentId, sportId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<SportAdviceDto>(sportAdvice));
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
        
        [Function(nameof(SportAdviceHttpTrigger.GetSportAdvicesByStudentId))]
        [OpenApiOperation(operationId: "GetSportAdvicesByStudentId", tags: new[] {"StudentOperations", "CoachOperations", "SportAdvice" }, Summary = "Get Sport Advices  By  student id", Description = "Getting sport advices using a student id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<SportAdviceDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Sport Advices not found", Description = "Sport Advices not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSportAdvicesByStudentId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "sportadvices/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        UserBase currentUser = _currentUserGetter.getCurrentUser(User, _userService);
                        //checking if current user is student
                        if (currentUser.UserRole == UserRoles.Student)
                        {
                            //if a student is logged in, they cannot view the details of another student
                            if (studentId != currentUser.UserId)
                            {
                                throw new Exception("Not allowed to view other student's sport advices");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.Forbidden);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.Forbidden;
                        return responseData;
                    }
                    
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        var sportAdvices = _sportStudentService.GetAllSportAdvicesByStudentId(studentId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<SportAdviceDto>>(sportAdvices));
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
        [Function(nameof(SportAdviceHttpTrigger.RemoveAllSportAdvicesByStudent))]
        [OpenApiOperation(operationId: "RemoveAllSportAdvicesByStudent", tags: new[] { "SportAdvice", "CoachOperations" }, Summary = "remove sport advices from a student", Description = "remove sport advices from a student.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(SportAdviceDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "student not found", Description = "student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> RemoveAllSportAdvicesByStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "sportadvices/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await userChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        _sportStudentService.RemoveSportAdvicesByStudentId(studentId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.NoContent);
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
