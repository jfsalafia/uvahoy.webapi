using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using uvahoy.Indicadores.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.UI;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System;

namespace uvahoy.Indicadores
{
    [AbpAllowAnonymous]
    [Abp.Web.Models.DontWrapResult]
    public class IndicadorAppService : uvahoyAppServiceBase, IIndicadorAppService
    {
        private readonly IRepository<Indicador> _indicadorRepository;
        private readonly IRepository<IndicadorUsuario> _indicadorUsuarioRepository;

        private readonly IRepository<Cotizacion> _cotizacionRepository;


        public IndicadorAppService(IRepository<Indicador> indicadorRepository, IRepository<IndicadorUsuario> indicadorUsuarioRepository, IRepository<Cotizacion> cotizacionRepository)
        {
            _indicadorRepository = indicadorRepository;
            _indicadorUsuarioRepository = indicadorUsuarioRepository;
            _cotizacionRepository = cotizacionRepository;
        }

        public ListResultDto<IndicadorListDto> GetList(GetIndicadorListInput input)
        {
            var indicadores = _indicadorRepository
                .GetAll()
                .Where(i => i.Nombre.Contains(input.Nombre) || input.Nombre == null)
                .OrderBy(e => e.Nombre)
                .ToList();

            return new ListResultDto<IndicadorListDto>(indicadores.ConvertAll(i => i.MapTo<IndicadorListDto>()).ToList());
        }

        public async Task CreateAsync(CreateIndicadorInput input)
        {
            var @indicador = Indicador.Create(input.Nombre, input.Descripcion, input.FuenteDatos, input.Abreviatura);
            await _indicadorRepository.InsertAsync(@indicador);
        }

        public async Task RegistrarIndicadorUsuario(EntityDto input)
        {
            var @indicador = await _indicadorRepository
                .GetAll()
                .Where(e => e.Id == input.Id)
                .FirstOrDefaultAsync();

            var @user = await GetCurrentUserAsync();

            await _indicadorUsuarioRepository.InsertAsync(IndicadorUsuario.Create(indicador, @user));
        }

        public IndicadorDetailOutput GetIndicadorDetail(IndicadorDetailInput input)
        {
            var @indicador = _indicadorRepository
                .GetAll()
                .Where(e => e.Id == input.IndicadorId)
                .FirstOrDefault();

            if (@indicador == null)
            {
                throw new UserFriendlyException("Indicador no encontrado.");
            }

            if (!input.FechaHasta.HasValue)
            {
                input.FechaHasta = input.FechaDesde.Date;
            }

            var dto = @indicador.MapTo<IndicadorDetailOutput>();

            var cotizacionesDB = _cotizacionRepository
                .GetAll()
                .Where(c => c.IndicadorId == input.IndicadorId && c.FechaHoraCotizacion >= input.FechaDesde && c.FechaHoraCotizacion <= input.FechaHasta)
                .ToList();

            var cotizaciones = new List<CotizacionDto>(cotizacionesDB.ConvertAll(c => c.MapTo<CotizacionDto>()));


            var diff = input.FechaHasta.Value.Date - input.FechaDesde.Date;

            if (cotizacionesDB.Count() <= diff.Days && diff.Days >= 0)
            {
                var cotizacionesCloud = GetCotizaciones(indicador.FuenteDatos, indicador.MetodoActualizacion, indicador.FormatoDatos, input.FechaDesde, input.FechaHasta.Value);
                var keys = cotizacionesDB.Select(cdb => FormatDate(cdb.FechaHoraCotizacion.Date));

                var faltantes = cotizacionesCloud.Where(c => !keys.Contains(FormatDate(c.Key.Date)));

                foreach (var vc in faltantes.Where(f => f.Value.HasValue))
                {
                    var cotDB = Cotizacion.Create(input.IndicadorId, vc.Key.Date, vc.Value.Value);
                    _cotizacionRepository.Insert(cotDB);

                    cotizaciones.Add(cotDB.MapTo<CotizacionDto>());
                }



            }

            for (var i = 0; i < diff.Days; i++)
            {
                var fechaBusqueda = input.FechaDesde.Date.AddDays(i);

                if (!cotizaciones.Any(cx => cx.IndicadorId == input.IndicadorId && cx.FechaHoraCotizacion == fechaBusqueda))
                {
                    var cotizacionPrevia = cotizaciones.LastOrDefault(p => p.IndicadorId == input.IndicadorId && p.FechaHoraCotizacion <= fechaBusqueda);

                    if (cotizacionPrevia != null)
                    {
                        cotizaciones.Add(new CotizacionDto()
                        {
                            CreationTime = cotizacionPrevia.CreationTime,
                            FechaHoraCotizacion = fechaBusqueda,
                            IndicadorId = cotizacionPrevia.IndicadorId,
                            ValorCotizacion = cotizacionPrevia.ValorCotizacion,
                            Id = cotizacionPrevia.Id * -1,
                            CreatorUserId = cotizacionPrevia.CreatorUserId
                        });
                    }
                    else
                    {
                        var cotizacionPreviaDb = _cotizacionRepository
                           .GetAll()
                           .LastOrDefault(c => c.IndicadorId == input.IndicadorId && c.FechaHoraCotizacion <= input.FechaDesde);

                        if (cotizacionPreviaDb != null)
                        {
                            var x = cotizacionPreviaDb.MapTo<CotizacionDto>();

                            x.FechaHoraCotizacion = fechaBusqueda;
                            cotizaciones.Add(x);
                        }
                    }
                }
            }

            dto.Cotizaciones = cotizaciones
                .Where(c => c.FechaHoraCotizacion >= input.FechaDesde && c.FechaHoraCotizacion <= input.FechaHasta.Value.Date)
                .OrderBy(c => c.FechaHoraCotizacion.Date)
                .ToList();

            return dto;

        }

        private static string FormatDate (DateTime d)
        {
            return string.Format("{0:dd/MM/yyyy}", d);
        }

        private IDictionary<DateTime, decimal?> GetCotizaciones(string url, string metodoActualizacion, string formatoDatos, DateTime fechaCotizacionDesde, DateTime fechaCotizacionHasta)
        {
            string requestFormat = url;

            var fechaCotizacionDesdeFormatted = FormatDate(fechaCotizacionDesde);

            var fechaCotizacionHastaFormatted = FormatDate(fechaCotizacionHasta);

            var req = string.Format(requestFormat, fechaCotizacionDesdeFormatted, fechaCotizacionHastaFormatted);

            string response = string.Empty;

            WebRequest request = WebRequest.Create(req);

            request.Method = metodoActualizacion;

            WebResponse wr = request.GetResponse();

            Stream receiveStream = wr.GetResponseStream();

            using (StreamReader reader = new StreamReader(receiveStream, System.Text.Encoding.UTF8))
            {
                if (formatoDatos == "HTML")
                {
                    return GetCotizacionesFromHTML(reader.ReadToEnd());
                }
                else
                {
                    return GetCotizacionesFromJSON(reader.ReadToEnd());
                }
            }
        }

        private IDictionary<DateTime, decimal?> GetCotizacionesFromJSON(string json)
        {
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
                DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local
            };

            var cotizacionJSONDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CotizacionJSONDTO>(json, jsonSerializerSettings);
            var items = new Dictionary<DateTime, decimal?>();

            if (cotizacionJSONDTO != null)
            {
                foreach (var cot in cotizacionJSONDTO.historico)
                {
                    items.Add(cot.Fecha, cot.Venta);
                }

                if (!items.ContainsKey(cotizacionJSONDTO.monedas.UltimaActualizacion.Date))
                {
                    items.Add(cotizacionJSONDTO.monedas.UltimaActualizacion.Date, cotizacionJSONDTO.monedas.Venta);
                }
            }
            return items;
        }

        private IDictionary<DateTime, decimal?> GetCotizacionesFromHTML(string html)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var contenidoNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='contenido']");
            var dicc = new Dictionary<DateTime, decimal?>();
            var diccOrden = new Dictionary<string, int>
            {
                { "FECHA", 0 },
                { "VALOR", 1 }
            };
            var globInfo = new System.Globalization.CultureInfo("es-AR");

            foreach (var contenidoNode in contenidoNodes)
            {
                var tablaNode = contenidoNode.SelectSingleNode("//table[@id='tabla']");

                if (tablaNode != null)
                {
                    var orden = 0;

                    var headNode = tablaNode.SelectSingleNode("./thead");

                    if (headNode != null)
                    {
                        foreach (var cabeceraNode in headNode.SelectNodes("./tr/th/b"))
                        {
                            var k = cabeceraNode.InnerText.ToUpper().Trim();
                            if (diccOrden.ContainsKey(k))
                            {
                                diccOrden[k] = orden;
                            }
                            orden++;
                        }
                    }

                    var filas = tablaNode.SelectNodes("./tr");
                    foreach (var fila in filas)
                    {
                        var key = string.Empty;
                        decimal value = decimal.Zero;
                        var ordenColumna = 0;

                        var columnas = fila.SelectNodes("./td");
                        foreach (var columna in columnas)
                        {
                            if (ordenColumna == diccOrden["FECHA"])
                            {
                                key = columna.InnerText.ToUpper();
                            }
                            else if (ordenColumna == diccOrden["VALOR"])
                            {
                                decimal.TryParse(columna.InnerText, System.Globalization.NumberStyles.Any, globInfo, out value);
                            }
                            ordenColumna++;
                        }

                        if (!string.IsNullOrEmpty(key) && value != decimal.Zero)
                        {
                            if (DateTime.TryParse(key, globInfo, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                            {
                                dicc.TryAdd(fecha, value);
                            }

                        }
                    }
                }
            }
            return dicc;
        }
    }

    internal class CotizacionJSONDTO
    {
        public CotizacionJSONDTO()
        {
            historico = new List<CotizacionJSONHistoricoDTO>().ToArray();
            monedas = new CotizacionJSONMonedaDTO();
        }

        public CotizacionJSONHistoricoDTO[] historico { get; set; }
        public CotizacionJSONMonedaDTO monedas { get; set; }
    }

    internal class CotizacionJSONMonedaDTO
    {
        public decimal VariacionPorcentual { get; set; }
        public decimal VariacionNeta { get; set; }
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
        public decimal Ultimo { get; set; }
        public decimal ValorCierreAnterior { get; set; }
        public string Icono { get; set; }
        public string Pais { get; set; }
        public string Tipo { get; set; }
        public string Ric { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public string Nombre { get; set; }
        public string UrlId { get; set; }
        public long Id { get; set; }
    }

    internal class CotizacionJSONHistoricoDTO
    {
        public long Id { get; set; }

        public decimal Compra { get; set; }

        public decimal Venta { get; set; }

        public string Ric { get; set; }

        public DateTime Fecha { get; set; }
    }
}
