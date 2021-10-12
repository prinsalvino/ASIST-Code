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
    public class CoachHttpTrigger
    {
        ILogger Logger { get; }

        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CoachHttpTrigger(ILogger<CoachHttpTrigger> logger, IMapper mapper, IUserService userService)
        {
            Logger = logger;
            _mapper = mapper;
            _userService = userService;
        }
        
        [Function(nameof(CoachHttpTrigger.GetCoaches))]
        [OpenApiOperation(operationId: "GetCoaches", tags: new[] { "CoachOperations", "Coach", "AdminOperations"}, Summary = "Get Coaches", Description = "Getting a list of coaches from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<CoachDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve coach list", Description = "Could not retrieve coach list")]
        public async Task<HttpResponseData> GetCoaches(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "coaches")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                
                var coaches = _userService.GetAll(UserRoles.Coach);
                
                await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<CoachDto>>(coaches));
                
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(CoachHttpTrigger.GetCoachesById))]
        [OpenApiOperation(operationId: "GetCoachesById", tags: new[] { "CoachOperations", "Coach", "AdminOperations"}, Summary = "Get Coach By Id", Description = "Getting a coach using a coachid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach to return", Description = "Id of coach to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CoachDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid coach id supplied", Description = "Invalid coach id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "coach not found", Description = "coach not found")]
        public async Task<HttpResponseData> GetCoachesById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                
                await response.WriteAsJsonAsync(_mapper.Map<CoachDto>(_userService.GetUser(coachId)));
                
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }

        [Function(nameof(CoachHttpTrigger.AddCoach))]
        [OpenApiOperation(operationId: "AddCoach", tags: new [] {"Coach", "CoachOperations", "AdminOperations"}, Summary = "Add a new coach", Description = "Add a new coach to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateCoachDto), Required = true, Description = "coach object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(CoachDto), Summary = "New coach details added", Description = "New coach details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid Input")]
        public async Task<HttpResponseData> AddCoach(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "coaches")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                
                CreateCoachDto createCoach = JsonConvert.DeserializeObject<CreateCoachDto>(requestBody);
                
                var coach = _mapper.Map<Coach>(createCoach);
                
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                
                _userService.AddUser(coach);
                
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }

        [Function(nameof(CoachHttpTrigger.UpdateCoach))]
        [OpenApiOperation(operationId:"UpdateCoach", tags:new [] { "CoachOperations", "Coach" }, Summary = "Update an existing coach", Description = "Update an existing coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach to return", Description = "Id of coach to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyCoachDto), Required = true, Description = "coach object that needs to be updated to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType:typeof(ModifyCoachDto), Summary = "coach details updated", Description = "coach details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "coach not found", Description = "coach not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdateCoach(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            try
            {
                Coach coach = (Coach)_userService.GetUser(coachId);
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ModifyCoachDto modifyCoach = JsonConvert.DeserializeObject<ModifyCoachDto>(requestBody);
            
                coach.FirstName = modifyCoach.FirstName;
                coach.LastName = modifyCoach.LastName;
                coach.EmailAddress = modifyCoach.EmailAddress;
                coach.Password = modifyCoach.Password;
                coach.PhoneNumber = modifyCoach.PhoneNumber;

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                _userService.UpdateUser(coach);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        /*[Function(nameof(CoachHttpTrigger.DeleteCoach))]
        [OpenApiOperation(operationId:"DeleteCoach", tags:new [] { "CoachOperations", "Coach", "AdminOperations" }, Summary = "delete an existing coach", Description = "delete an existing coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach to delete", Description = "Id of coach to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "coach not found", Description = "coach not found")]
        public async Task<HttpResponseData> DeleteCoach(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            try
            {
                _userService.DeleteUser(_userService.GetUser(coachId));
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }*/
    }
}