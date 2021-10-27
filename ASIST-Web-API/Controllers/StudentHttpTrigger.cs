using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ASIST_Project_Web_API.UserChecker;
using ASIST_Web_API.Attributes;
using ASIST_Web_API.DTO;
using ASIST_Web_API.Helpers;
using AutoMapper;
using Azure;
using Azure.Core;
using Domain;
using Microsoft.AspNetCore.Http;
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
    public class StudentHttpTrigger
    {
        private readonly IUserService _userService;
        private readonly IOrganisationService _orgService;

        private readonly IMapper _mapper;
        UserChecker UserChecker;
        CurrentUserGetter CurrentUserGetter;
        ILogger Logger { get; }

        public StudentHttpTrigger(ILogger<StudentHttpTrigger> logger, IUserService userService, IMapper mapper, IOrganisationService organisationService)
        {
            Logger = logger;
            _userService = userService;
            _mapper = mapper;
            UserChecker = new UserChecker(logger);
            CurrentUserGetter = new CurrentUserGetter();
            _orgService = organisationService;
        }
        
        [Function(nameof(StudentHttpTrigger.GetStudents))]
        [OpenApiOperation(operationId: "GetStudents", tags: new[] {"Student", "CoachOperations"}, Summary = "Get Students", Description = "Getting a list of students from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<StudentDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "no students found", Description = "no student found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetStudents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "students")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                    UserBase currentUser = CurrentUserGetter.getCurrentUser(User, _userService);
                    IEnumerable<UserBase> students;
                    
                    // If role is coach only get students from the coach list of organisations
                    if (currentUser.UserRole == UserRoles.Coach)
                    {
                        try
                        {
                            IEnumerable<Organisation> orgs = _orgService.GetOrganisationsByCoachId(currentUser.UserId).ToList();
                            students = _userService.GetStudentsOfCoachOrganisation(orgs);
                        }
                        catch (Exception e)
                        {
                            HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                            await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                                "No students found"));
                            responseData.StatusCode = HttpStatusCode.NotFound;
                            return responseData;
                        }
                    }
                    else // Role is admin, get all
                    {
                        students = _userService.GetAllByRole(UserRoles.Student);
                    }            
                    await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<StudentDto>>(students));
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
        [Function(nameof(StudentHttpTrigger.GetStudentsById))]
        [OpenApiOperation(operationId: "GetStudentsById", tags: new[] { "StudentOperations", "CoachOperations", "Student" }, Summary = "Get Student By Id", Description = "Getting a student using a studentid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(StudentDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetStudentsById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await UserChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    UserBase currentUser = CurrentUserGetter.getCurrentUser(User, _userService);

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
                        var student = _userService.GetUser(studentId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<StudentDto>(student));
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

        [Function(nameof(StudentHttpTrigger.AddStudent))]
        [OpenApiOperation(operationId: "AddStudent", tags: new [] { "StudentOperations", "Student"}, Summary = "Add a new student", Description = "Add a new student to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateStudentDto), Required = true, Description = "Student object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType:"application/json", bodyType: typeof(CreateStudentDto), Summary = "New student details added", Description = "New student details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid Input fields entered")]
        public async Task<HttpResponseData> AddStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "students")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateStudentDto createStudent = JsonConvert.DeserializeObject<CreateStudentDto>(requestBody);

                if (createStudent.OrganisationId == 0 || createStudent.OrganisationId == null)
                {
                    //assigning organisation 9 (which is "no organisation") to the student if the organisation id is 0
                    createStudent.OrganisationId = 9;
                }
                
                //checking if organisation exists
                long organisationId = createStudent.OrganisationId.Value;
                _orgService.CheckIfOrganisationExists(organisationId);


                if (!createStudent.IsValid(validationResults: out var validationResults))
                {
                    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                    await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(), $"student is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}"));
                        
                    //forcing status code to 400
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }

                try
                {
                    
                    int age = (DateTime.Now.Subtract(createStudent.DateOfBirth.Value).Days) / 365;

                    if (age <= 6 || age >= 13)
                    {
                        throw new Exception("Sorry, only children aged from 6 to 13 can register");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                var student = _mapper.Map<Student>(createStudent);
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                _userService.AddUser(student);
                
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

        [Function(nameof(StudentHttpTrigger.UpdateStudent))]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiOperation(operationId:"UpdateStudent", tags:new [] { "StudentOperations", "Student" }, Summary = "Update an existing student", Description = "Update an existing student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyStudentDto), Required = true, Description = "Student object that needs to be updated to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType:typeof(StudentDto), Summary = "Student details updated", Description = "Student details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid input fields entered")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> UpdateStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await UserChecker.ExecuteForStudent(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    UserBase currentUser = CurrentUserGetter.getCurrentUser(User, _userService);

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
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }
                    
                    Student student = (Student)_userService.GetUser(studentId);

                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    ModifyStudentDto modifyStudent = JsonConvert.DeserializeObject<ModifyStudentDto>(requestBody);
                    
                    if (modifyStudent.FirstName != String.Empty)
                    {
                        student.FirstName = modifyStudent.FirstName;
                    }
                    if (modifyStudent.LastName != String.Empty)
                    {
                        student.LastName = modifyStudent.LastName;
                    }
                    if (modifyStudent.EmailAddress != String.Empty)
                    {
                        if (new EmailAddressAttribute().IsValid(modifyStudent.EmailAddress))
                        {
                            student.EmailAddress = modifyStudent.EmailAddress;
                        }
                        else
                        {
                            throw new Exception("Invalid email address type");
                        }
                    }
                    if (modifyStudent.Password != String.Empty)
                    {
                        modifyStudent.Password = BCrypt.Net.BCrypt.HashPassword(modifyStudent.Password);
                        student.Password = modifyStudent.Password;
                    }
                    if (modifyStudent.OrganisationId != null)
                    {
                        long organisationId = modifyStudent.OrganisationId.Value;
                        _orgService.CheckIfOrganisationExists(organisationId);
                        student.OrganisationId = modifyStudent.OrganisationId;
                    }
                    
                    _userService.UpdateUser(student);

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
        [Function(nameof(StudentHttpTrigger.DeleteStudent))]
        [OpenApiOperation(operationId:"DeleteStudent", tags:new [] { "StudentOperations", "Student"}, Summary = "delete an existing student", Description = "delete an existing student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to delete", Description = "Id of student to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent, Summary = "student has been deleted", Description = "student has been deleted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> DeleteStudent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            return await UserChecker.ExecuteForStudent(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var foundUser = _userService.GetUser(studentId);
                        _userService.CheckIfUserIsStudent(studentId);
                        _userService.DeleteUser(foundUser);
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