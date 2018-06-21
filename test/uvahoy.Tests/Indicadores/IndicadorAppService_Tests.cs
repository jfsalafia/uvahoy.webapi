using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using uvahoy.Indicadores.Dto;
using uvahoy.Indicadores;

namespace uvahoy.Tests.Indicadores
{
    public class IndicadorAppService_Tests : uvahoyTestBase
    {
        private readonly IIndicadorAppService _appService;

        public IndicadorAppService_Tests()
        {
            _appService = Resolve<IIndicadorAppService>();
        }

        [Fact]
        public void GetIndicadores_Test()
        {
            // Act
            var output = _appService.GetList(new GetIndicadorListInput() { Nombre = "UVA", PaisId = 1 });

            // Assert
            output.Items.Count.ShouldBe(1);
        }

        //[Fact]
        //public async Task CreateIndicador_Test()
        //{
        //    var url = @"http://www.bcra.gov.ar/PublicacionesEstadisticas/Principales_variables_datos.asp?desde={0}&hasta={1}&fecha=Fecha_Cvs&descri=22&campo=Cvs&primeravez=1&alerta=5";
        //    // Act
        //    await _appService.CreateAsync(
        //        new CreateIndicadorInput
        //        {
        //            Abreviatura = "UVA",
        //            Nombre = "Unidad de Valor Adquisitivo",
        //            Descripcion = "Unidad de Valor Adquisitivo. ARG",
        //            FuenteDatos = url
        //        });

        //    await UsingDbContextAsync(async context =>
        //    {
        //        var item = await context.Indicadores.FirstOrDefaultAsync(u => u.Abreviatura == "UVA");
        //        item.ShouldNotBeNull();
        //    });
        //}



        [Fact]
        public async Task GetIndicadorDetail_Test()
        {
            await UsingDbContextAsync(async context =>
            {
                var item = await context.Indicadores.FirstOrDefaultAsync(u => u.Abreviatura == "uva");

                // Act
                var detail = _appService.GetIndicadorDetail(
                     new IndicadorDetailInput
                     {
                         FechaDesde = System.DateTime.Now,
                         FechaHasta = System.DateTime.Now,
                         IndicadorId = item.Id
                     });

                item.ShouldNotBeNull();
                detail.Cotizaciones.Count.ShouldBe(1);
            });
        }


        [Fact]
        public async Task RegistrarIndicadorUsuario_Test()
        {
            await UsingDbContextAsync(async context =>
            {
                var item = await context.Indicadores.FirstOrDefaultAsync(u => u.Abreviatura == "uva");

                // Act
                var detail = _appService.RegistrarIndicadorUsuario(
                     new EntityDto
                     {
                         Id = item.Id
                     });

                item.ShouldNotBeNull();
            });
        }
    }
}
