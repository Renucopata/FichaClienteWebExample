using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FichaCliente.Models;
using FichaCliente.Scripts;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace FichaCliente.Controllers
{
    [Authorize]
    public class FichasController : Controller
    {
        Consultas QUERY = new Consultas();
        Procedimientos STORED = new Procedimientos();
        private readonly IMemoryCache _cache;

        public FichasController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void GetExtraCaja(string rec)
        {
            List<(string, string)> info = QUERY.GetEdad(rec);
            if (info.Count == 0)
            {
                ViewBag.edad = "";
                ViewBag.antiguedad = "Sin Fecha";
            }
            else
            {
                ViewBag.edad = Math.Round(Double.Parse(info[0].Item1, CultureInfo.GetCultureInfo("en-US")));
                ViewBag.antiguedad = info[0].Item2;
            }
        }

        public void GetExtraCartera(string rec)
        {
            List<(string, Int64)> info = QUERY.GetTelefono(rec);
            if (info.Count == 0)
            {
                ViewBag.direccion = "";
                ViewBag.telefono = "";
            }
            else
            {
                ViewBag.direccion = info[0].Item1;
                ViewBag.telefono = info[0].Item2;
            }
        }

        public void GetExtraRetencion(string rec)
        {
            string info = QUERY.consultaRetenciones(rec);
            if (info == "")
                info = "Sin retenciones";
            ViewBag.retenciones = info;
        }

        [HttpGet]
        public IActionResult Paneles(List<ModeloResumen>? clientes)
        {
            if (_cache.TryGetValue("KeyPanel", out List<ModeloResumen> recuperados))
            {
                clientes = recuperados;
                ViewData["titular"] = clientes.ElementAt(0).Nombre_Titular;
                ViewData["ci"] = clientes.ElementAt(0).Documento_de_Identidad;
                ViewData["titular"] = clientes.ElementAt(0).Nombre_Titular;
                _cache.Remove("KeyPanel");
            }
            else
            {
                clientes = clientes == null ? new List<ModeloResumen>() : clientes;
            }
            return View(clientes);
        }

        [HttpPost]
        public IActionResult Paneles(string DocId, string NomTit)
        {
            if (DocId == null)
                DocId = "-";
            if (NomTit == null)
                NomTit = "-";
            HttpContext.Session.SetString("CIsesion", DocId);
            HttpContext.Session.SetString("TITsesion", NomTit);
            List<ModeloResumen> panel = STORED.DatosCliente(DocId, NomTit);

            if (panel.FirstOrDefault() != null)
            {
                ViewData["ci"] = panel.ElementAt(0).Documento_de_Identidad;
                ViewData["titular"] = panel.ElementAt(0).Nombre_Titular;
            }
            else
            {
                ViewData["ci"] = "";
                ViewData["titular"] = "";

            }
            if (panel.Count == 0)
                ViewData["alert"] = true;
            else
                ViewData["alert"] = false;
            return Paneles(panel);
        }

        public IActionResult PizarraCartera()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            string rec = "";
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            if (recID != "-")
                rec = recID;
            else
                rec = recTIT;
            List<ModeloCartera> creditos = STORED.DatosCartera(recID, recTIT);
            GetExtraCaja(rec);
            GetExtraRetencion(rec);
            return View(creditos);
        }

        public IActionResult PizarraCajas()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            string rec = "";
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            if (recID != "-")
                rec = recID;
            else
                rec = recTIT;
            List<ModeloCaja> cajas = STORED.DatosCajas(recID, recTIT);
            GetExtraCartera(rec);
            GetExtraCaja(rec);
            GetExtraRetencion(rec);
            List<(string, string)> barra = QUERY.GetBarraCAJA(rec);
            var categorias = new List<string>();
            var series = new List<Double>();
            foreach (var tuple in barra)
            {
                categorias.Add(tuple.Item1);
                series.Add(Double.Parse(tuple.Item2, CultureInfo.GetCultureInfo("en-US")));
            }
            ViewBag.CategoriasCaja = categorias;
            ViewBag.SeriesCaja = series;
            return View(cajas);
        }

        public IActionResult PizarraDpfs()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            string rec = "";
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            if (recID != "-")
                rec = recID;
            else
                rec = recTIT;
            List<ModeloDpf> dpfs = STORED.DatosDpf(recID, recTIT);
            GetExtraCaja(rec);
            GetExtraCartera(rec);
            GetExtraRetencion(rec);
            List<int> IdDpf = new List<int>();
            List<int> Faltantes = new List<int>();
            List<int> Completados = new List<int>();
            int c = 0;
            foreach (var dp in dpfs)
            {
                c++;
                IdDpf.Add(c);
                var apertura = dp.Fecha_de_Vencimiento.AddDays(-dp.Plazo);
                var comp = (int)(DateTime.Now - apertura).TotalDays;
                var falt = dp.Plazo - comp;
                Faltantes.Add(falt);
                Completados.Add(comp);

            }
            ViewBag.categoriasDPF = IdDpf;
            ViewBag.diasCDPF = Completados;
            ViewBag.diasFDPF = Faltantes;
            return View(dpfs);
        }

        public IActionResult PizarraTarjetas()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            string rec = "";
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            if (recID != "-")
                rec = recID;
            else
                rec = recTIT;
            ModeloTarjetas tarjetas = STORED.DatosTarjeta(recID, recTIT);
            GetExtraCaja(rec);
            GetExtraCartera(rec);
            GetExtraRetencion(rec);
            string vigven = tarjetas.Interes_Vigente_Bolivianos + "/" + tarjetas.Interes_Vencido_Bolivianos;
            ViewBag.interes = vigven;
            ViewBag.importe = tarjetas.Importe_Capital_Bolivianos;
            ViewBag.saldo = tarjetas.Saldo_Cartera_Bolivianos;
            ViewBag.tasa = tarjetas.Tasa;
            if (tarjetas.Fecha_de_incumplimiento.ToString() == "01/01/1999")
                ViewBag.Finc = "Sin fecha";
            else
                ViewBag.Finc = tarjetas.Fecha_de_incumplimiento.ToString("d/M/yyyy");
            if (tarjetas.Fecha_de_Vencimiento.ToString() == "01/01/1999")
                ViewBag.Venc = "Sin fecha";
            else
                ViewBag.Venc = tarjetas.Fecha_de_Vencimiento.ToString("d/M/yyyy");
            return View(tarjetas);
        }

        public IActionResult PizarraCorr()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            string rec = "";
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            if (recID != "-")
                rec = recID;
            else
                rec = recTIT;
            List<ModeloCorr> cuentas = STORED.DatosCuentas(recID, recTIT);
            GetExtraCaja(rec);
            GetExtraCartera(rec);
            GetExtraRetencion(rec);
            List<(string, string)> barra = QUERY.GetBarraCUENTAS(rec);
            var categorias = new List<string>();
            var series = new List<Double>();
            foreach (var tuple in barra)
            {
                categorias.Add(tuple.Item1);
                series.Add(Double.Parse(tuple.Item2, CultureInfo.GetCultureInfo("en-US")));
            }
            ViewBag.CategoriasCuentas = categorias;
            ViewBag.SeriesCuentas = series;
            return View(cuentas);
        }

        public IActionResult Regreso()
        {
            string? recID = HttpContext.Session.GetString("CIsesion");
            string? recTIT = HttpContext.Session.GetString("TITsesion");
            if (recID == null)
                recID = "-";
            if (recTIT == null)
                recTIT = "-";
            List<ModeloResumen> DATA = STORED.DatosCliente(recID, recTIT);
            _cache.Set("KeyPanel", DATA);
            return RedirectToAction("Paneles");
        }
    }
}
