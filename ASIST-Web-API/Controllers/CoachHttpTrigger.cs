using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ASIST_Project_Web_API.UserChecker;
using ASIST_Project_Web_API.Utils;
using ASIST_Web_API.Attributes;
using ASIST_Web_API.DTO;
using ASIST_Web_API.Helpers;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
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
    public class CoachHttpTrigger
    {
        ILogger Logger { get; }

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        CurrentUserGetter CurrentUserGetter;
        private readonly IOrganisationService _organisationService;
        UserChecker UserChecker { get; }

        public CoachHttpTrigger(ILogger<CoachHttpTrigger> logger, IMapper mapper, IUserService userService, IOrganisationService organisationService)
        {
            Logger = logger;
            _mapper = mapper;
            _userService = userService;
            UserChecker = new UserChecker(logger);
            CurrentUserGetter = new CurrentUserGetter();
            _organisationService = organisationService;
        }
        
        [AsistAuth]
        [OpenApiSecurity("AsistAuth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [Function(nameof(CoachHttpTrigger.GetCoaches))]
        [OpenApiOperation(operationId: "GetCoaches", tags: new[] { "CoachOperations", "Coach"}, Summary = "Get Coaches", Description = "Getting a list of coaches from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<CoachDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "No coaches found", Description = "No coaches found")]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetCoaches(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "coaches")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var coaches = _userService.GetAllByRole(UserRoles.Coach);
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
        [Function(nameof(CoachHttpTrigger.GetCoachesById))]
        [OpenApiOperation(operationId: "GetCoachesById", tags: new[] { "CoachOperations", "Coach"}, Summary = "Get Coach By Id", Description = "Getting a coach using a coachid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach", Description = "Id of coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CoachDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid coach id supplied", Description = "Invalid coach id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "coach not found", Description = "coach not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetCoachesById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsCoach(coachId);
                        var coach = _userService.GetUser(coachId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<CoachDto>(coach));
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

        [Function(nameof(CoachHttpTrigger.AddCoach))]
        [OpenApiOperation(operationId: "AddCoach", tags: new [] {"Coach", "CoachOperations"}, Summary = "Add a new coach", Description = "Add a new coach to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateCoachDto), Required = true, Description = "coach object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType:"application/json", bodyType: typeof(CoachDto), Summary = "New coach details added", Description = "New coach details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid Input fields entered")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AddCoach(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "coaches")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await UserChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                    CreateCoachDto createCoach = JsonConvert.DeserializeObject<CreateCoachDto>(requestBody);

                    if (!createCoach.IsValid(validationResults: out var validationResults))
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(), $"Coach is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}"));
                        
                        //forcing status code to 400
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        return responseData;
                    }
                    
                    var coach = _mapper.Map<Coach>(createCoach);

                    HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);

                    _userService.AddUser(coach);
                    
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

        [Function(nameof(CoachHttpTrigger.UpdateCoach))]
        [OpenApiOperation(operationId:"UpdateCoach", tags:new [] { "CoachOperations", "Coach" }, Summary = "Update an existing coach", Description = "Update an existing coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach", Description = "Id of coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyCoachDto), Required = true, Description = "coach object that needs to be updated to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType:typeof(ModifyCoachDto), Summary = "coach details updated", Description = "coach details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid input fields entered")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "coach not found", Description = "coach not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> UpdateCoach(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "coaches/{coachId}")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    UserBase currentUser = CurrentUserGetter.getCurrentUser(User, _userService);
                    //checking if current user is coach
                    if (currentUser.UserRole == UserRoles.Coach)
                    {
                        //if a coach is logged in, they cannot change the details of another coach
                        if (coachId != currentUser.UserId)
                        {
                            return req.CreateResponse(HttpStatusCode.Forbidden);
                        }
                    }

                    try
                    {
                        _userService.CheckIfUserIsCoach(coachId);
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }
                    
                    
                    Coach coach = (Coach)_userService.GetUser(coachId);
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    ModifyCoachDto modifyCoach = JsonConvert.DeserializeObject<ModifyCoachDto>(requestBody);

                    if (modifyCoach.FirstName != String.Empty)
                    {
                        coach.FirstName = modifyCoach.FirstName;
                    }

                    if (modifyCoach.LastName != String.Empty)
                    {
                        coach.LastName = modifyCoach.LastName;
                    }

                    if (modifyCoach.EmailAddress != String.Empty)
                    {
                        if (new EmailAddressAttribute().IsValid(modifyCoach.EmailAddress))
                        {
                            coach.EmailAddress = modifyCoach.EmailAddress;
                        }
                        else
                        {
                            throw new Exception("Invalid email address, check example: something@email.com");
                        }
                    }

                    if (modifyCoach.Password != String.Empty)
                    {
                        modifyCoach.Password = BCrypt.Net.BCrypt.HashPassword(modifyCoach.Password);
                        coach.Password = modifyCoach.Password;
                    }

                    if (modifyCoach.PhoneNumber != String.Empty)
                    {
                        if (new PhoneAttribute().IsValid(modifyCoach.PhoneNumber))
                        {
                            coach.PhoneNumber = modifyCoach.PhoneNumber;
                        }
                        else
                        {
                            throw new Exception("Invalid phone number, check example: 0612345678");
                        }
                    }

                    HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                    _userService.UpdateUser(coach);

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
        [Function(nameof(CoachHttpTrigger.GetOrganisationsByCoachId))]
        [OpenApiOperation(operationId: "GetOrganisationsByCoachId", tags: new[] { "Coach", "CoachOperations" }, Summary = "Get Organisations By coach Id", Description = "Getting organisations using a coach id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "coachId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of coach", Description = "Id of coach", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrganisationDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid coach id supplied", Description = "Invalid coach id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "organisations not found", Description = "organisations not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetOrganisationsByCoachId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "coaches/{coachId}/organisations")] HttpRequestData req,
            FunctionContext executionContext, long coachId)
        {
            return await UserChecker.ExecuteCoach(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var organisations = _organisationService.GetOrganisationsByCoachId(coachId);
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