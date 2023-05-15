namespace FichaCliente.Models
{
    public class ModeloResumen
    {
        public string? Documento_de_Identidad { get; set; }
        public string? Nombre_Titular { get; set; }
        public string? Tipo_de_Credito { get; set; }
        public decimal Monto_Desembolsado_en_Bolivianos { get; set; }
        public decimal Saldo_en_Bolivianos { get; set; }
        public char Calificacion { get; set; }
        public string? Tipo_de_Caja_de_Ahorro { get; set; }
        public string? Estado { get; set; }
        public string? Tipo_de_Dpf_Actual { get; set; }
        public decimal Monto_de_Capital_Bolivianos { get; set; }
        public DateTime Fecha_de_Vencimiento { get; set; }
        public int Plazo { get; set; }
        public string? Tipo_de_Cuenta_Corriente { get; set; }
        public string? Estado_CC { get; set; }
        public string? Tipo { get; set; }
        public decimal Importe_Capital_Bolivianos { get; set; }
        public decimal Saldo_Cartera_Bolivianos { get; set; }
    }
}
