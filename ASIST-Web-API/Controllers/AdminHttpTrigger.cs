using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ASIST_Project_Web_API.UserChecker;
using ASIST_Web_API.Attributes;
using ASIST_Web_API.DTO;
using ASIST_Web_API.Helpers;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    public class AdminHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        UserChecker userChecker;
        
        public AdminHttpTrigger(ILogger<AdminHttpTrigger> logger, IMapper mapper, IUserService userService)
        {
            Logger = logger;
            _mapper = mapper;
            _userService = userService;
            userChecker = new UserChecker(logger);
        }
        
        [Function(nameof(AdminHttpTrigger.GetAdmins))]
        [OpenApiOperation(operationId: "GetAdmins", tags: new[] { "Admin"}, Summary = "Get admins", Description = "Getting a list of admins from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<AdminDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve admin list", Description = "Could not retrieve admin list")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetAdmins(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "v2/admins")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await userChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                    await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<AdminDto>>((_userService.GetAllByRole(UserRoles.Admin))));

                    return response;
                }
                catch (Exception e)
                {
                    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                    return responseData;
                }
            });
            
        }
        [Function(nameof(AdminHttpTrigger.GetAdminById))]
        [OpenApiOperation(operationId: "GetAdminById", tags: new[] { "Admin"}, Summary = "Get admin By Id", Description = "Getting an admin using an adminid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin", Description = "Id of admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AdminDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid admin id supplied", Description = "Invalid admin id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> GetAdminById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            return await userChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        var admin = _userService.GetUser(adminId);
                        _userService.CheckIfUserIsAdmin(adminId);
                        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteAsJsonAsync(_mapper.Map<AdminDto>(admin));
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
                    await responseData.WriteAsJsonAsync(
                        new ErrorResponse(responseData.StatusCode.ToString(), e.Message));
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
            });
        }
        [Function(nameof(AdminHttpTrigger.AddAdmin))]
        [OpenApiOperation(operationId: "AddAdmin", tags: new [] { "Admin"}, Summary = "Add a new admin", Description = "Add a new admin to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateAdminDto), Required = true, Description = "admin object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType:"application/json", bodyType: typeof(AdminDto), Summary = "New admin details added", Description = "New admin details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input data", Description = "Invalid Input data")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> AddAdmin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "v2/admins")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return await userChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    CreateAdminDto createAdmin = JsonConvert.DeserializeObject<CreateAdminDto>(requestBody);

                    if (!createAdmin.IsValid(validationResults: out var validationResults))
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(), $"Admin is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}"));
                        
                        //forcing status code to 400
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        return responseData;
                    }
                    
                    var admin = _mapper.Map<Admin>(createAdmin);
                    HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);

                    _userService.AddUser(admin);

                    return response;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                    return responseData;
                }
            });
        }
        
        [Function(nameof(AdminHttpTrigger.UpdateAdmin))]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin", Description = "Id of admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiOperation(operationId:"UpdateAdmin", tags:new [] { "Admin" }, Summary = "Update an existing admin", Description = "Update an existing admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyAdminDto), Required = true, Description = "Admin object that needs to be updated to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType:typeof(AdminDto), Summary = "admin details updated", Description = "admin details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input fields entered", Description = "Invalid input fields entered")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> UpdateAdmin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            return await userChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsAdmin(adminId);
                    }
                    catch (Exception e)
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.NotFound);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(),
                            e.Message));
                        responseData.StatusCode = HttpStatusCode.NotFound;
                        return responseData;
                    }

                    Admin admin = (Admin)_userService.GetUser(adminId);
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    ModifyAdminDto modifyAdmin = JsonConvert.DeserializeObject<ModifyAdminDto>(requestBody);

                    if (!modifyAdmin.IsValid(validationResults: out var validationResults))
                    {
                        HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                        await responseData.WriteAsJsonAsync(new ErrorResponse(responseData.StatusCode.ToString(), $"Admin is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage))}"));
                        
                        //forcing status code to 400
                        responseData.StatusCode = HttpStatusCode.BadRequest;
                        return responseData;
                    }
                    
                    if (modifyAdmin.FirstName != String.Empty)
                    {
                        admin.FirstName = modifyAdmin.FirstName;
                    }
                    if (modifyAdmin.LastName != String.Empty)
                    {
                        admin.LastName = modifyAdmin.LastName;
                    }
                    if (modifyAdmin.EmailAddress != String.Empty)
                    {
                        if (new EmailAddressAttribute().IsValid(modifyAdmin.EmailAddress))
                        {
                            admin.EmailAddress = modifyAdmin.EmailAddress;
                        }
                        else
                        {
                            throw new Exception("Invalid email address");
                        }
                    }
                    if (modifyAdmin.Password != String.Empty)
                    {
                        modifyAdmin.Password = BCrypt.Net.BCrypt.HashPassword(modifyAdmin.Password);
                        admin.Password = modifyAdmin.Password;
                    }

                    HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                    _userService.UpdateUser(admin);

                    return response;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    HttpResponseData responseData = req.CreateResponse(HttpStatusCode.BadRequest);
                    await responseData.WriteAsJsonAsync(
                        new ErrorResponse(responseData.StatusCode.ToString(), e.Message));
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                    return responseData;
                }
            });
            
        }
        [Function(nameof(AdminHttpTrigger.DeleteAdmin))]
        [OpenApiOperation(operationId:"DeleteAdmin", tags:new [] { "Admin" }, Summary = "delete an existing admin", Description = "delete an existing admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin", Description = "Id of admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent, Summary = "Admin Successfully deleted", Description = "Admin Successfully deleted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        [AsistAuth]
        [ForbiddenResponse]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> DeleteAdmin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            return await userChecker.ExecuteAdmin(req, executionContext, async (ClaimsPrincipal User) =>
            {
                try
                {
                    try
                    {
                        _userService.CheckIfUserIsAdmin(adminId);
                        _userService.DeleteUser(_userService.GetUser(adminId));
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
                    await responseData.WriteAsJsonAsync(
                        new ErrorResponse(responseData.StatusCode.ToString(), e.Message));
                    return responseData;
                }
            });  
        }
    }
}