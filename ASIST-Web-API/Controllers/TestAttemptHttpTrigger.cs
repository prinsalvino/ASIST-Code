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
    public class TestAttemptHttpTrigger
    {
        ILogger Logger;
        UserChecker UserChecker;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITestAttemptService _testAttemptService;
        CurrentUserGetter _currentUserGetter;
        public TestAttemptHttpTrigger(ILogger<TestAttemptHttpTrigger> logger, IMapper mapper, IUserService userService, ITestAttemptService testAttemptService)
        {
            Logger = logger;
            UserChecker = new UserChecker(logger);
            _testAttemptService = testAttemptService;
            _userService = userService;
            _mapper = mapper;
            _currentUserGetter = new CurrentUserGetter();
        }
        
        [Function(nameof(TestAttemptHttpTrigger.GetTestsAttemptedByStudentId))]
        [OpenApiOperation(operationId: "GetTestsAttemptedByStudentId", tags: new[] {"StudentOperations", "CoachOperations", "TestAttempt" }, Summary = "Get tests attempted By  student id", Description = "Getting tests attempted by a student from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TestAttemptDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "test attempt not found", Description = "test attempt not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetTestsAttemptedByStudentId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "testsattempted/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await UserChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    UserBase currentUser = _currentUserGetter.getCurrentUser(User, _userService);

                    if (currentUser.UserRole == UserRoles.Student)
                    {
                        if (studentId != currentUser.UserId)
                        {
                            return req.CreateResponse(HttpStatusCode.Forbidden);
                        }
                    }
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        var testsAttempted = _testAttemptService.GetAllTestsAttemptedByStudentId(studentId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<TestAttemptDto>>(testsAttempted));
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
        [Function(nameof(TestAttemptHttpTrigger.RemoveAllTestsAttemptedByStudent))]
        [OpenApiOperation(operationId: "RemoveAllTestsAttemptedByStudent", tags: new[] { "TestAttempt", "CoachOperations" }, Summary = "remove tests attempted from a student", Description = "remove tests attempted from a student.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(TestAttemptDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "student not found", Description = "student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> RemoveAllTestsAttemptedByStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "testsattempted/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        _testAttemptService.RemoveAllTestsAttemptedByStudentId(studentId);
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