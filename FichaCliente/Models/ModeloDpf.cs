namespace FichaCliente.Models
{
    public class ModeloDpf
    {
        public string? Numero_de_Idetificacion { get; set; }
        public string? Codigo_de_Nit { get; set; }
        public string? Nombre_de_la_Cuenta { get; set; }
        public string? Tipo_de_Dpf_Actual { get; set; }
        public decimal Monto_de_Capital_Bolivianos { get; set; }
        public int Plazo { get; set; }
        public DateTime Fecha_de_Vencimiento { get; set; }
        public double Tasa_de_Interes { get; set; }
    }
}
