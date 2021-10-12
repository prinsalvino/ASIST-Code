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
    public class AdminHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        
        public AdminHttpTrigger(ILogger<AdminHttpTrigger> logger, IMapper mapper, IUserService userService)
        {
            Logger = logger;
            _mapper = mapper;
            _userService = userService;
        }
        
        [Function(nameof(AdminHttpTrigger.GetAdmins))]
        [OpenApiOperation(operationId: "GetAdmins", tags: new[] { "Admin", "AdminOperations"}, Summary = "Get admins", Description = "Getting a list of admins from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<AdminDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve admin list", Description = "Could not retrieve admin list")]
        public async Task<HttpResponseData> GetAdmins(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v2/admins")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<AdminDto>>((_userService.GetAll(UserRoles.Admin))));

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(AdminHttpTrigger.GetAdminById))]
        [OpenApiOperation(operationId: "GetAdminById", tags: new[] { "Admin", "AdminOperations" }, Summary = "Get admin By Id", Description = "Getting an admin using a adminid from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin to return", Description = "Id of admin to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AdminDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid admin id supplied", Description = "Invalid admin id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        public async Task<HttpResponseData> GetAdminById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(_mapper.Map<AdminDto>(_userService.GetUser(adminId)));

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(AdminHttpTrigger.AddAdmin))]
        [OpenApiOperation(operationId: "AddAdmin", tags: new [] { "Admin", "AdminOperations"}, Summary = "Add a new admin", Description = "Add a new admin to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateAdminDto), Required = true, Description = "admin object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(AdminDto), Summary = "New admin details added", Description = "New admin details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid Input")]
        public async Task<HttpResponseData> AddAdmin(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "v2/admins")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateAdminDto createAdmin = JsonConvert.DeserializeObject<CreateAdminDto>(requestBody);

                var admin = _mapper.Map<Admin>(createAdmin);
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            
                _userService.AddUser(admin);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        
        [Function(nameof(AdminHttpTrigger.UpdateAdmin))]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin to return", Description = "Id of admin to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiOperation(operationId:"UpdateAdmin", tags:new [] { "Admin", "AdminOperations" }, Summary = "Update an existing admin", Description = "Update an existing admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(ModifyAdminDto), Required = true, Description = "Admin object that needs to be updated to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType:typeof(AdminDto), Summary = "admin details updated", Description = "admin details updated in the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdateAdmin(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            try
            {
                Admin admin = (Admin)_userService.GetUser(adminId);
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
                ModifyAdminDto modifyAdmin = JsonConvert.DeserializeObject<ModifyAdminDto>(requestBody);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                admin.FirstName = modifyAdmin.FirstName;
                admin.LastName = modifyAdmin.LastName;
                admin.EmailAddress = modifyAdmin.EmailAddress;
                admin.Password = modifyAdmin.Password;
            
                _userService.UpdateUser(admin);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(AdminHttpTrigger.DeleteAdmin))]
        [OpenApiOperation(operationId:"DeleteAdmin", tags:new [] { "Admin", "AdminOperations" }, Summary = "delete an existing admin", Description = "delete an existing admin", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "adminId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of admin to delete", Description = "Id of admin to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "admin not found", Description = "admin not found")]
        public async Task<HttpResponseData> DeleteAdmin(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "v2/admins/{adminId}")] HttpRequestData req,
            FunctionContext executionContext, long adminId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                _userService.DeleteUser(_userService.GetUser(adminId));

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