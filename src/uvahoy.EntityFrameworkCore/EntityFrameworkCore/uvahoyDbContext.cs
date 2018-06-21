using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using uvahoy.Authorization.Roles;
using uvahoy.Authorization.Users;
using uvahoy.MultiTenancy;

namespace uvahoy.EntityFrameworkCore
{
    public class uvahoyDbContext : AbpZeroDbContext<Tenant, Role, User, uvahoyDbContext>
    {
        public virtual DbSet<Indicadores.Indicador> Indicadores { get; set; }

        public virtual DbSet<Indicadores.IndicadorUsuario> IndicadoresUsuario { get; set; }
        
        public virtual DbSet<Indicadores.Cotizacion> Cotizaciones { get; set; }

        public uvahoyDbContext(DbContextOptions<uvahoyDbContext> options)
            : base(options)
        {
        }
    }
}
