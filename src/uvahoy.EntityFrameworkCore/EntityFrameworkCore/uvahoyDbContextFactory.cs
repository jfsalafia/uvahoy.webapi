using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using uvahoy.Configuration;
using uvahoy.Web;

namespace uvahoy.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class uvahoyDbContextFactory : IDesignTimeDbContextFactory<uvahoyDbContext>
    {
        public uvahoyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<uvahoyDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            uvahoyDbContextConfigurer.Configure(builder, configuration.GetConnectionString(uvahoyConsts.ConnectionStringName));

            return new uvahoyDbContext(builder.Options);
        }
    }
}
