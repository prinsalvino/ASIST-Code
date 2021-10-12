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
    public class FinalScoreHttpTrigger
    {
        ILogger logger { get; set; }
        public FinalScoreHttpTrigger(ILogger<FinalScoreHttpTrigger> logger)
        {
            this.logger = logger;
        }

        [Function(nameof(FinalScoreHttpTrigger.GetFinalScoreByStudentsId))]
        [OpenApiOperation(operationId: "GetFinalScoreByStudentsId", tags: new[] { "StudentOperations", "CoachOperations", "Student" }, Summary = "Get final score by StudentsId", Description = "Getting a final score using a studentid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to return", Description = "Id of student to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Student), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid student id supplied", Description = "Invalid student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student not found", Description = "Student not found")]
        public async Task<HttpResponseData> GetFinalScoreByStudentsId(
          [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "students/{studentId}/finalscore")] HttpRequestData req,
          FunctionContext executionContext, long studentId)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            Student student = new Student();

            await response.WriteAsJsonAsync(student);

            return response;
        }
    }
}
