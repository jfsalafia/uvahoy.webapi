using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using uvahoy.Web;

namespace uvahoy.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class uvahoyDbContextFactory : IDesignTimeDbContextFactory<uvahoyDbContext>
    {
        public uvahoyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<uvahoyDbContext>();
            var configuration = Configuration.AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            string cnnString = configuration.GetConnectionString(uvahoyConsts.ConnectionStringName);

            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                cnnString = configuration.GetConnectionString("defaultConnection");


            uvahoyDbContextConfigurer.Configure(builder, cnnString);

            return new uvahoyDbContext(builder.Options);
        }
    }
}
