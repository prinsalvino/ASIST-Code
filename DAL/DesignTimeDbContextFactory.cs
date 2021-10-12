using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ASIST.Repository
{
    internal class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<SportingContext>
    {
        public SportingContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SportingContext>();
            builder.UseSqlServer("Server=tcp:asist-project-server.database.windows.net,1433;Initial Catalog=asist-project-db;Persist Security Info=False;User ID=asistadmin;Password=Projectadmin101;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var context = new SportingContext(builder.Options);
            return context;
        }
    }
}