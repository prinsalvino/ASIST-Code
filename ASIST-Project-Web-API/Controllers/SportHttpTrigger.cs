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
using System.Text;
using System.Threading.Tasks;

namespace ASIST.Controllers
{
    public class SportHttpTrigger
    {
        ILogger logger; 
        Coach CoachContext { get; }
        public SportHttpTrigger(ILogger<SportHttpTrigger> logger, Coach coachContext)
        {
            this.logger = logger;
            CoachContext = coachContext;
        }

        //Getting sports advices for all students?
        /*[Function(nameof(SportHttpTrigger.GetSportsAdvices))]
        [OpenApiOperation(operationId: "GetSportsAdvices", tags: new[] { "Student", "Coach" }, Summary = "Get Sports advices", Description = "Getting a list of sports advices from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Student>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve sport advice list", Description = "Could not retrieve sport advice list")]
        public async Task<HttpResponseData> GetSportsAdvices(
          [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "students/sportsadvices")] HttpRequestData req,
          FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            List<Student> students = new List<Student>();

            foreach (Student s in students)
            {
                await response.WriteAsJsonAsync(s.Advices);
            }

            return response;
        }*/

        [Function(nameof(SportHttpTrigger.GetSportAdviceBySportId))]
        [OpenApiOperation(operationId: "GetSportAdviceBySportId", tags: new[] { "StudentOperations", "CoachOperations", "SportAdvice" }, Summary = "Get Sport Advice  By Sport Id", Description = "Getting a sport advice using a sport id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "sportId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of sport to return", Description = "Id of sport to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Sport), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid sport id supplied", Description = "Invalid sport id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Sport Advice not found", Description = "Sport Advice not found")]
        public async Task<HttpResponseData> GetSportAdviceBySportId(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "sportadvices/{studentId}/{sportId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            Student student = new Student();

           /*foreach (SportAdvice advice in student)
            {
                await response.WriteAsJsonAsync(advice);
            }*/

            return response;
        }
        
        [Function(nameof(SportHttpTrigger.GetSportAdvicesByStudentId))]
        [OpenApiOperation(operationId: "GetSportAdvicesByStudentId", tags: new[] {"StudentOperations", "CoachOperations", "SportAdvice" }, Summary = "Get Sport Advice  By  student id", Description = "Getting a sport advice using a student id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Sport>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Sport Advice not found", Description = "Sport Advice not found")]
        public async Task<HttpResponseData> GetSportAdvicesByStudentId(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "sportadvices/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            Student student = new Student();

            await response.WriteAsJsonAsync(student);

            return response;
        }

        
    }
}
