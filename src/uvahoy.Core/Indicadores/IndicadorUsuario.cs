using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using uvahoy.Authorization.Users;

namespace uvahoy.Indicadores
{
    [Table("IndicadorUsuario")]
    public class IndicadorUsuario : Entity
    {

        [ForeignKey("IndicadorId")]
        public virtual Indicador Indicador { get; protected set; }
        public virtual int EventId { get; protected set; }

        [ForeignKey("UserId")]
        public virtual User User { get; protected set; }
        public virtual long UserId { get; protected set; }

        protected IndicadorUsuario()
        {

        }

        public static IndicadorUsuario Create(Indicador @indicador, User user)
        {
            return new IndicadorUsuario
            {
                EventId = @indicador.Id,
                Indicador = @indicador,
                UserId = @user.Id,
                User = user
            };
        }

        public async Task CancelAsync(IRepository<IndicadorUsuario> repository)
        {
            if (repository == null) { throw new ArgumentNullException("repository"); }

            await repository.DeleteAsync(this);
        }
    }
}
