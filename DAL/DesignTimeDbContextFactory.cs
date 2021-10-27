using System;
using System.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace DAL
{
    internal class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<SportingContext>
    {
         
        public SportingContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SportingContext>();
            

            IConfigurationRoot configuration = new ConfigurationBuilder()

                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString())
                .AddJsonFile("ASIST-Web-API/bin/Debug/net5.0/appsettings.json")
                .Build();
            
            //difficult to use environment variables if the appsettings.json file is in another project. Hence, used the connection string here. Also tried linking it here, but that wouldn't work either
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            
            var context = new SportingContext(builder.Options);
            return context;
        }
    }
}