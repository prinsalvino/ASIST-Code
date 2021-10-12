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
    public class OrganisationsHttpTrigger
    {
        ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrganisationService _organisationService;
        
        public OrganisationsHttpTrigger(ILogger<OrganisationsHttpTrigger> logger, IOrganisationService organisationService, IMapper mapper)
        {
            _logger = logger;
            _organisationService = organisationService;
            _mapper = mapper;
        }
        
        [Function(nameof(OrganisationsHttpTrigger.AddOrganisation))]
        [OpenApiOperation(operationId: "AddOrganisation", tags: new [] { "Organisation", "AdminOperations"}, Summary = "Add a new organisation", Description = "Add a new organisation to the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType:typeof(CreateOrganisationDto), Required = true, Description = "organisation object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"application/json", bodyType: typeof(OrganisationDto), Summary = "New organisation details added", Description = "New organisation details added to the database")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid Input")]
        public async Task<HttpResponseData> AddOrganisation(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "organisations")] HttpRequestData req,
            FunctionContext executionContext)
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
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
        }
        [Function(nameof(OrganisationsHttpTrigger.GetOrganisations))]
        [OpenApiOperation(operationId: "GetOrganisations", tags: new[] { "Organisation","AdminOperations"}, Summary = "Get Organisations", Description = "Getting a list of organisations from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OrganisationDto>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Could not retrieve organisation list", Description = "Could not retrieve organisation list")]
        public async Task<HttpResponseData> GetOrganisations(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "organisations")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                var organisations = _organisationService.GetAll();
                await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<OrganisationDto>>(organisations));
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not perform request");
            }
            
        }
        [Function(nameof(OrganisationsHttpTrigger.GetOrganisationsById))]
        [OpenApiOperation(operationId: "GetOrganisationsById", tags: new[] { "Organisation", "AdminOperations" }, Summary = "Get Organisation By Id", Description = "Getting an organisation using a organisation id from the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "organisationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of organisation to return", Description = "Id of organisation to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrganisationDto), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid organisation id supplied", Description = "Invalid organisation id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "organisation not found", Description = "organisation not found")]
        public async Task<HttpResponseData> GetOrganisationsById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "organisations/{organisationId}")] HttpRequestData req,
            FunctionContext executionContext, long organisationId)
        {
            try
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(_mapper.Map<OrganisationDto>(_organisationService.GetOrganisationById(organisationId)));
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