using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace uvahoy.EntityFrameworkCore
{
    public static class uvahoyDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<uvahoyDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<uvahoyDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
