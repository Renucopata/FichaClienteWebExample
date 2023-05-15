namespace FichaCliente.Models
{
    public class ModeloCorr
    {
        public Int64 Nro_de_Cuenta { get; set; }
        public string? Numero_de_Identificacion { get; set; }
        public string? Numero_de_Nit { get; set; }
        public string? Nombre_Cliente { get; set; }
        public string? Tipo_de_Caja_de_Ahorro { get; set; }
        public decimal Saldo_en_Bolivianos { get; set; }
        public double Tasa { get; set; }
        public string? Estado { get; set; }
    }
}
