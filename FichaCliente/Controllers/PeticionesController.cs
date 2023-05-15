using Microsoft.AspNetCore.Mvc;
using FichaCliente.Models;
using FichaCliente.Scripts;

namespace FichaCliente.Controllers
{
    public class PeticionesController : Controller
    {
        Procedimientos DETALLES = new Procedimientos();
        Consultas COMBOX = new Consultas();

        [HttpPost]
        public JsonResult DetallesCartera(string operacionCRE)
        {
            ModeloCartera credito = DETALLES.DetalleCreditos(operacionCRE);
            return Json(new { respCre = credito });
        }

        [HttpGet]
        public JsonResult ComboCI()
        {
            List<string> docIdentidad = COMBOX.consultaDOC();
            return Json(new { combox = docIdentidad });
        }

        [HttpGet]
        public JsonResult ComboTIT()
        {
            List<string> nomTitular = COMBOX.consultaNOM();
            return Json(new { combox = nomTitular });
        }
    }
}
