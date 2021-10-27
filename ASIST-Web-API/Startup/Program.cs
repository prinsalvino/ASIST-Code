using System;
using System.Configuration;
using System.IO;
using ASIST_Web_API.Helpers;
using DAL;
using DAL.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServiceLayer;
using ServiceLayer.SecurityConfig;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Web_API.Startup {
    public class Program {

        public static IConfigurationRoot Configuration { get; set; }

        public static void Main() {
            IHost host = new HostBuilder()
                 .ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
                     Builder.UseNewtonsoftJson().UseMiddleware<JwtMiddleware>();
                 })
                 .ConfigureOpenApi()
                 .ConfigureServices(Configure)
                 .Build();
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            host.Run();
        }

        static void Configure(HostBuilderContext Builder, IServiceCollection Services) {

            Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
            Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
            Services.AddSingleton<IUserService, UserService>();
            Services.AddSingleton<IUserRepository, UserRepository>();
            Services.AddSingleton<ISkillStudentRepository, SkillStudentRepository>();
            Services.AddSingleton<ISkillStudentService, SkillStudentService>();
            Services.AddSingleton<ITokenService, TokenService>();
            Services.AddSingleton<ISportStudentRepository, SportStudentRepository>();
            Services.AddSingleton<ISportRepository, SportRepository>();
            Services.AddSingleton<ISkillRepository, SkillRepository>();
            Services.AddSingleton<ISportService, SportService>();
            Services.AddSingleton<ISkillService, SkillService>();
            Services.AddSingleton<ITestAttemptRepository, TestAttemptRepository>();
            Services.AddSingleton<ITestAttemptService, TestAttemptService>();
            Services.AddSingleton<ISportStudentService, SportStudentService>();
            Services.AddSingleton<IOrganisationRepository, OrganisationRepository>();
            Services.AddSingleton<IOrganisationService, OrganisationService>();
            
            Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            //using ServiceLifetime.Transient to allow multiple instances of the dbcontext when calling services
            Services.AddDbContext<SportingContext>((Services,Builder) 
                => Builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

        }
    }
}