using ASIST.Repository;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ASIST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SportingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Server=tcp:asist-project-server.database.windows.net,1433;Initial Catalog=asist-project-db;Persist Security Info=False;User ID=asistadmin;Password=Projectadmin101;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")));
        }
    }
}