using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ASIST.DTO;
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
    public class SkillHttpTrigger
    {
        ILogger Logger { get; }

        private readonly IMapper _mapper;
        private readonly ISkillService _skillService;
        private readonly IUserService _userService;
        public SkillHttpTrigger(ILogger<SkillHttpTrigger> logger, IMapper mapper, ISkillService skillService, IUserService userService)
        {
            Logger = logger;
            _mapper = mapper;
            _skillService = skillService;
            _userService = userService;
        }
        
        
        
        [Function(nameof(SkillHttpTrigger.AddSkillLapTimes))]
        [OpenApiOperation(operationId: "AddSkillLapTimes", tags: new[] { "CoachOperations" , "SkillPerformed" }, Summary = "Add a new skill laptimes", Description = "Add new skill laptimes to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to post", Description = "Id of student to post", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<CreateSkillDto>), Required = true, Description = "skillperformed object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SkillDto), Summary = "New skill laptimes added", Description = "New skill laptimes added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid Input")]
        public async Task<HttpResponseData> AddSkillLapTimes(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "skillsperformed/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId)
        {
            try
            {
                var student = _userService.GetUser(studentId);
                if (student == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    IEnumerable<CreateSkillDto> createSkills = JsonConvert.DeserializeObject<IEnumerable<CreateSkillDto>>(requestBody);
                    List<SkillStudent> skills = new List<SkillStudent>();
            
                    foreach (CreateSkillDto skill in createSkills)
                    {
                        skill.StudentId = studentId;
                        skill.TimeTaken = TimeSpan.Parse(skill.Time);
                        skill.TimeOfCompletion = skill.TimeTaken.Ticks;
                        skills.Add(_mapper.Map<SkillStudent>(skill));
                    }
            
                    _skillService.AddSkills(skills);
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }
        
        [Function(nameof(SkillHttpTrigger.GetSkillsPerformedByStudentId))]
        [OpenApiOperation(operationId: "GetSkillsPerformedByStudentId", tags: new[] { "StudentOperations", "CoachOperations", "SkillPerformed" }, Summary = "Get skills by student id", Description = "Getting a list of skills by student id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student to post", Description = "Id of student to post", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<SkillDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve skills list", Description = "Could not retrieve skills list")]
        public async Task<HttpResponseData> GetSkillsPerformedByStudentId(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "skillsperformed/{studentId}")] HttpRequestData req,
            FunctionContext executionContext,long studentId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<SkillDto>>(_skillService.GetAll(studentId)));

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