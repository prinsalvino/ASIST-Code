using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Newtonsoft.Json;
using ServiceLayer;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Controllers
{
    public class OrganisationsHttpTrigger
    {
        ILogger Logger;
        private readonly IMapper _mapper;
        private readonly IOrganisationService _organisationService;
        private readonly IUserService _userService;
        UserChecker UserChecker;

        public OrganisationsHttpTrigger(ILogger<OrganisationsHttpTrigger> logger,
            IOrganisationService organisationService, IMapper mapper
            , IUserService userService)
        {
            Logger = logger;
            _organisationService = organisationService;
            _mapper = mapper;
            _userService = userService;
            UserChecker = new UserChecker(logger);
        }
        
        [Function(nameof(OrganisationsHttpTrigger.AddOrganisation))]
        [OpenApiOperation(operationId: "AddOrganisation", tags: new [] { "Organisation"}, Summary = "Add a new organisation", Description = "Add a new organisation to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateOrganisationDto), Required = true, Description = "organisation object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType:"application/json", bodyType: typeof(OrganisationDto), Summary = "New organisation details added", Description = "New organisation details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid Input fields entered")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AddOrganisation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "organisations")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await UserChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    CreateOrganisationDto createOrganisation = JsonConvert.DeserializeObject<CreateOrganisationDto>(requestBody);

                    var organisation = _mapper.Map<Organisation>(createOrganisation);

                    HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                    _organisationService.AddOrganisation(organisation);

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
        [Function(nameof(OrganisationsHttpTrigger.GetOrganisations))]
        [OpenApiOperation(operationId: "GetOrganisations", tags: new[] { "Organisation", "StudentOperations", "CoachOperations"}, Summary = "Get Organisations", Description = "Getting a list of organisations from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OrganisationDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve organisation list", Description = "Could not retrieve organisation list")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetOrganisations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "organisations")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await UserChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var organisations = _organisationService.GetAll();
                        _organisationService.CheckIfOrganisationListIsEmpty(organisations);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<OrganisationDto>>(organisations));
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
        [Function(nameof(OrganisationsHttpTrigger.GetOrganisationsById))]
        [OpenApiOperation(operationId: "GetOrganisationsById", tags: new[] { "Organisation", "CoachOperations" }, Summary = "Get Organisation By Id", Description = "Getting an organisation using a organisation id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of organisation", Description = "Id of organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrganisationDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid organisation id supplied", Description = "Invalid organisation id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "organisation not found", Description = "organisation not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetOrganisationsById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "organisations/{organisationId}")] HttpRequestData req,
            FunctionContext executionContext, long organisationId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _organisationService.CheckIfOrganisationExists(organisationId);
                        var organisation = _organisationService.GetOrganisationById(organisationId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<OrganisationDto>(organisation));
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
        [Function(nameof(OrganisationsHttpTrigger.GetStudentsByOrganisationId))]
        [OpenApiOperation(operationId: "GetStudentsByOrganisationId", tags: new[] { "CoachOperations", "Organisation" }, Summary = "Get students By organisation Id", Description = "Getting students using an organisation id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of an organisation", Description = "ID of an organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(StudentDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid organisation id supplied", Description = "Invalid organisation id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Student(s) not found", Description = "Student(s) not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetStudentsByOrganisationId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "organisations/{organisationId}/students")] HttpRequestData req,
            FunctionContext executionContext, long organisationId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _organisationService.CheckIfOrganisationExists(organisationId);
                        var students = _userService.GetStudentsByOrganisationId(organisationId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<StudentDto>>(students));
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
        [Function(nameof(OrganisationsHttpTrigger.GetCoachesByOrganisation))]
        [OpenApiOperation(operationId: "GetCoachesByOrganisation", tags: new[] { "Organisation", "CoachOperations"}, Summary = "Get coaches By organisation id", Description = "Getting a list of coaches using a organisation id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of organisation", Description = "Id of organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CoachDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid organisation id supplied", Description = "Invalid organisation id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "no coaches found for this organisation", Description = "no coaches found for this organisation")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetCoachesByOrganisation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "organisations/{organisationId}/coaches")] HttpRequestData req,
            FunctionContext executionContext, long organisationId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var coaches = _userService.GetCoachesByOrganisationId(organisationId);
                        _userService.CheckIfUsersListIsEmpty(coaches);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<CoachDto>>(coaches));
                        return response;
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            "No coaches found"));
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
        [Function(nameof(OrganisationsHttpTrigger.AssignStudentToOrganisation))]
        [OpenApiOperation(operationId: "AssignStudentToOrganisation", tags: new[] { "CoachOperations", "Organisation" }, Summary = "assign student to an organisation", Description = "assign student to an organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "studentId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of student", Description = "Id of student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of an organisation", Description = "ID of an organisation to assign to a student", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(StudentDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid organisation or student id supplied", Description = "Invalid organisation or student id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "organisation or student not found", Description = "organisation or student not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AssignStudentToOrganisation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "organisations/{organisationId}/students/{studentId}")] HttpRequestData req,
            FunctionContext executionContext, long studentId, long organisationId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) => 
            { 
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsStudent(studentId);
                        _organisationService.CheckIfOrganisationExists(organisationId);
                        _organisationService.AssignStudentToOrganisation(studentId, organisationId);
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
        [Function(nameof(OrganisationsHttpTrigger.AssignCoachToOrganisation))]
        [OpenApiOperation(operationId: "AssignCoachToOrganisation", tags: new[] { "Organisation" }, Summary = "assign coach to organisation", Description = "assigning a coach to an organisation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach", Description = "Id of coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of organisation", Description = "Id of organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(OrganisationDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid coach or organisation id supplied", Description = "Invalid coach or organisation id supplied")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AssignCoachToOrganisation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "organisations/{organisationId}/coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId, long organisationId)
        {
            return await UserChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsCoach(coachId);
                        _organisationService.AssignCoachToOrganisation(coachId,organisationId);
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
        
        [Function(nameof(OrganisationsHttpTrigger.UnAssignCoachFromOrganisation))]
        [OpenApiOperation(operationId: "UnAssignCoachFromOrganisation", tags: new[] { "Organisation" }, Summary = "remove coach from an organisation", Description = "un-assigning a coach from an organisation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach", Description = "Id of coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of organisation", Description = "Id of organisation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "application/json", bodyType: typeof(OrganisationDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid coach or organisation id supplied", Description = "Invalid coach or organisation id supplied")]
        public async Task<HttpResponseData> UnAssignCoachFromOrganisation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "organisations/{organisationId}/coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId, long organisationId)
        {
            return await UserChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsCoach(coachId);
                        _organisationService.UnAssignCoachFromOrganisation(coachId,organisationId);
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