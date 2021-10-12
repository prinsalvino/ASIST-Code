using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASIST.DTO;
using ASIST.Helpers;
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

namespace ASIST.Controllers
{
    public class StudentHttpTrigger
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        ILogger Logger { get; }

        public StudentHttpTrigger(ILogger<StudentHttpTrigger> logger, IUserService userService, IMapper mapper)
        {
            Logger = logger;
            _userService = userService;
            _mapper = mapper;
        }
        
        [Function(nameof(StudentHttpTrigger.GetStudents))]
        [Authorize]
        [OpenApiOperation(operationId: "GetStudents", tags: new[] { "CoachOperations"}, Summary = "Get Students", Description = "Getting a list of students from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<StudentDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve student list", Description = "Could not retrieve student list")]
        public async Task<HttpResponseData> GetStudents(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "students")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                var students = _userService.GetAll(UserRoles.Student);
                await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<StudentDto>>(students));
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(StudentHttpTrigger.GetStudentsById))]
        [OpenApiOperation(operationId: "GetStudentsById", tags: new[] { "StudentOperations", "CoachOperations", "Student" }, Summary = "Get Student By Id", Description = "Getting a student using a studentid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(StudentDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        public async Task<HttpResponseData> GetStudentsById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(_mapper.Map<StudentDto>(_userService.GetUser(studentId)));
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }

        [Function(nameof(StudentHttpTrigger.AddStudent))]
        [OpenApiOperation(operationId: "AddStudent", tags: new [] { "StudentOperations", "Student"}, Summary = "Add a new student", Description = "Add a new student to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateStudentDto), Required = true, Description = "Student object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(CreateStudentDto), Summary = "New student details added", Description = "New student details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid Input")]
        public async Task<HttpResponseData> AddStudent(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "students")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateStudentDto createStudent = JsonConvert.DeserializeObject<CreateStudentDto>(requestBody);

                var student = _mapper.Map<Student>(createStudent);
                
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                _userService.AddUser(student);
                
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }

        [Function(nameof(StudentHttpTrigger.UpdateStudent))]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiOperation(operationId:"UpdateStudent", tags:new [] { "StudentOperations", "Student" }, Summary = "Update an existing student", Description = "Update an existing student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyStudentDto), Required = true, Description = "Student object that needs to be updated to the database")]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType:typeof(StudentDto), Summary = "Student details updated", Description = "Student details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdateStudent(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            try
            {
                Student student = (Student)_userService.GetUser(studentId);

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ModifyStudentDto modifyStudent = JsonConvert.DeserializeObject<ModifyStudentDto>(requestBody);

                student.EmailAddress = modifyStudent.EmailAddress;
                student.Password = modifyStudent.Password;
                student.OrganisationId = modifyStudent.OrganisationId;

                _userService.UpdateUser(student);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }
        [Function(nameof(StudentHttpTrigger.DeleteStudent))]
        [OpenApiOperation(operationId:"DeleteStudent", tags:new [] { "StudentOperations", "Student", "AdminOperations" }, Summary = "delete an existing student", Description = "delete an existing student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to delete", Description = "Id of student to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        public async Task<HttpResponseData> DeleteStudent(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            try
            {
                Student foundStudent = (Student)_userService.GetUser(studentId);
                 _userService.DeleteUser(foundStudent);
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }
    }
}