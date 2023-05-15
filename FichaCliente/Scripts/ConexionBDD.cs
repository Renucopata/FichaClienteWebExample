namespace FichaCliente.Scripts
{
    public class ConexionBDD
    {
        private String cadConexion = String.Empty;
        private String cadRetenciones = String.Empty;
        public ConexionBDD()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            cadConexion = builder.GetSection("ConnectionStrings:ConexionBDD").Value;
            cadRetenciones=builder.GetSection("ConnectionStrings:ConexionRETENCIONES").Value;
        }
        public String Get_cadConexion()
        {
            return cadConexion;
        }

        public String Get_Retenciones()
        {
            return cadRetenciones;
        }
    }
}
