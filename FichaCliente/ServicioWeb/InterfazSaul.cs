using FichaCliente.Models;

namespace FichaCliente.ServicioWeb
{
    public interface InterfazSaul
    {
        public ModeloUsuario Autenticacion(string usuario, string cont);
        bool Cargos(Int64 agenda);
    }
}
