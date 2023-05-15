namespace FichaCliente.Models
{
    public class ModeloTarjetas
    {
        public string? Nombre_Cliente { get; set; }
        public string? Nro_CI { get; set; }
        public char Calificacion { get; set; }
        public string? Estado { get; set; }
        public decimal Importe_Capital_Bolivianos { get; set; }
        public decimal Saldo_Cartera_Bolivianos { get; set; }
        public double Interes_Vigente_Bolivianos { get; set; }
        public double Interes_Vencido_Bolivianos { get; set; }
        public string? Tipo { get; set; }
        public double Tasa { get; set; }
        public DateTime Fecha_de_Vencimiento { get; set; }
        public int Dias_de_Incumplimiento { get; set; }
        public DateTime Fecha_de_incumplimiento { get; set; }
        public string? Estado_Tarjeta { get; set; }
    }
}
