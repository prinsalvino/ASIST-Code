using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ServiceLayer.Formula;
using ASIST_Project_Web_API.UserChecker;
using ASIST_Web_API.Attributes;
using ASIST_Web_API.DTO;
using ASIST_Web_API.Helpers;
using AutoMapper;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ServiceLayer;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Controllers
{
    public class SkillPerformedHttpTrigger
    {
        ILogger Logger { get; }

        private readonly IMapper _mapper;
        private readonly ISkillStudentService _skillStudentService;
        private readonly IUserService _userService;
        private readonly ISkillService _skillService;
        private readonly ISportStudentService _sportStudent;
        private readonly ITestAttemptService _testAttemptService;
        UserChecker userChecker;
        CurrentUserGetter CurrentUserGetter;
        public SkillPerformedHttpTrigger(ILogger<SkillPerformedHttpTrigger> logger, IMapper mapper, ISkillStudentService skillStudentService, IUserService userService, ISkillService skillService, ISportStudentService sportStudentService, ITestAttemptService testAttemptService)
        {
            Logger = logger;
            _mapper = mapper;
            _skillStudentService = skillStudentService;
            _userService = userService;
            userChecker = new UserChecker(logger);
            _skillService = skillService;
            _sportStudent = sportStudentService;
            _testAttemptService = testAttemptService;
            CurrentUserGetter = new CurrentUserGetter();
        }
        
        
        
        [Function(nameof(SkillPerformedHttpTrigger.AddSkillLapTimes))]
        [OpenApiOperation(operationId: "AddSkillLapTimes", tags: new[] { "CoachOperations" , "SkillPerformed" }, Summary = "Add new skill laptimes", Description = "Add new skill laptimes for a student to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<CreateSkillStudentDto>), Required = true, Description = "skillperformed object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(SkillStudentDto), Summary = "New skill laptimes added", Description = "New skill laptimes added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid Input fields entered")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AddSkillLapTimes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "skillsperformed/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await userChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }
                    
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    IEnumerable<CreateSkillStudentDto> createSkills = JsonConvert.DeserializeObject<IEnumerable<CreateSkillStudentDto>>(requestBody).ToList();
                    
                    List<SkillStudent> skills = new List<SkillStudent>();
                    var student = (Student)_userService.GetUser(studentId);
                    int age = (DateTime.Now.Subtract(student.DateOfBirth).Days) / 365;
                    
                    if (!createSkills.IsValid(validationResults: out var validationResults))
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(), $"Skill is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}"));
                    
                        //forcing status code to 400
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        return responseData;
                    }

                    try
                    {
                        if (createSkills.Count() != 5)
                        {
                            throw new Exception("Please enter only 5 skills at a time");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    try
                    {
                        if (age <= 6 || age >= 13)
                        {
                            throw new Exception(
                                "Calculations can only be performed for children between and including the ages 6 to 13");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                    foreach (CreateSkillStudentDto skill in createSkills)
                    {
                        try
                        {
                            _userService.CheckIfUserIsCoach(skill.CoachId);
                            _skillService.GetSkillById(skill.SkillId);
                        }
                        catch (Exception e)
                        {
                            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                            await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                                e.Message));
                            responseData.StatusCode = HttpStatusCode.NotFound;
                            return responseData;
                        }
                        skill.StudentId = studentId;
                        skill.TimeTaken = TimeSpan.Parse(skill.Time);
                        skill.TimeOfCompletion = skill.TimeTaken.Ticks;
                        skills.Add(_mapper.Map<SkillStudent>(skill));
                    }

                    var skillsPerformed = skills.Select(c => c.TimeOfCompletion).ToList();

                    List<int> times = new List<int>();
                    
                    //converting "string timespan" to "int seconds" for formula calculations
                    foreach(var timeOfCompletion in skillsPerformed)
                    {
                        TimeSpan tp = TimeSpan.FromTicks(timeOfCompletion);
                        int time = (int)tp.TotalSeconds;
                        times.Add(time);
                    }

                    int tiger = times[0];
                    int sprint = times[1];
                    int ballHandling = times[2];
                    int rolling = times[3];
                    int agility = times[4];


                    _sportStudent.AddSportAdvices(student, age, tiger, sprint, ballHandling, rolling, agility);
                    _skillStudentService.AddSkillLapTimes(skills, student.Gender, age, tiger, sprint, ballHandling, rolling, agility);
                    _testAttemptService.AddTestAttempt(student, age, tiger, sprint, ballHandling, rolling, agility);
                    
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.NoContent);

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
            });
        }
        
        [Function(nameof(SkillPerformedHttpTrigger.GetSkillsPerformedByStudentId))]
        [OpenApiOperation(operationId: "GetSkillsPerformedByStudentId", tags: new[] { "StudentOperations", "CoachOperations", "SkillPerformed" }, Summary = "Get skills by student id", Description = "Getting a list of skills by student id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<CreateSkillStudentDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve skills list", Description = "Could not retrieve skills list")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetSkillsPerformedByStudentId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "skillsperformed/{studentId}")] HttpRequestData req,
            FunctionContext executionContext,long studentId)
        {
            return await userChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        UserBase currentUser = CurrentUserGetter.getCurrentUser(User, _userService);
                        //checking if current user is student
                        if (currentUser.UserRole == UserRoles.Student)
                        {
                            //if a student is logged in, they cannot view the details of another student
                            if (studentId != currentUser.UserId)
                            {
                                throw new Exception("Not allowed to view other student's skills");
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
                        var skills = _skillStudentService.GetAll(studentId);
                        _skillStudentService.CheckIfSkillStudentListIsEmpty(skills);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<SkillStudentDto>>(skills));
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
        
        [Function(nameof(SkillPerformedHttpTrigger.RemoveAllSkillsPerformedByStudent))]
        [OpenApiOperation(operationId: "RemoveAllSkillsPerformedByStudent", tags: new[] { "SkillPerformed", "CoachOperations" }, Summary = "remove skills performed from a student", Description = "remove skills performed from a student.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(SkillStudentDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "student not found", Description = "student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> RemoveAllSkillsPerformedByStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "skillsperformed/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await userChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        _skillStudentService.RemoveAllSkillsPerformedByStudentId(studentId);
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