using System.Configuration;
using ASIST.Helpers;
using ASIST.Repository;
using ASIST.Helpers;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer;
using Repository_IOrganisationRepository = ASIST.Repository.IOrganisationRepository;
using ServiceLayer_IOrganisationService = ServiceLayer.IOrganisationService;

namespace ASIST
{
    public class Program
    {


        public static void Main()
        {
            IHost host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureServices(Configure)
                .Build();

            host.Run();
        }

        static void Configure(HostBuilderContext Builder, IServiceCollection Services)
        {

            Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
            Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
            Services.AddSingleton<IUserService, UserService>();
            Services.AddSingleton<IUserRepository, UserRepository>();
            Services.AddSingleton<ISkillRepository, SkillRepository>();
            Services.AddSingleton<ISkillService, SkillService>();
            Services.AddSingleton<IOrganisationRepository, OrganisationRepository>();
            Services.AddSingleton<IOrganisationService, OrganisationService>();
            Services.AddSingleton<AppSettings>();
            Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            Services.AddDbContextPool<SportingContext>((Services, Builder)
                => Builder.UseSqlServer("Server=tcp:asist-project-server.database.windows.net,1433;Initial Catalog=asist-project-db;Persist Security Info=False;User ID=asistadmin;Password=Projectadmin101;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            
        }
    }
}