namespace FichaCliente.Models
{
    public class ModeloCartera
    {
        public Int64 Numero_de_Prestamo { get; set; }
        public string? Documento_de_Identidad { get; set; }
        public string? Nombre_Titular { get; set; }
        public string? Direccion { get; set; }
        public string? Tipo_de_Credito { get; set; }
        public decimal Monto_Desembolsado_en_Bolivianos { get; set; }
        public decimal Saldo_en_Bolivianos { get; set; }
        public char Calificacion { get; set; }
        public int Cantidad_de_Reprogramaciones { get; set; }
        public DateTime Fecha_de_Reprogramaciones { get; set; }
        public string? Destino { get; set; }
        public int Dias_de_Incumplimiento { get; set; }
        public int Dias_de_Pase_a_Vencido { get; set; }
        public string? Plazo { get; set; }
        public DateTime Fecha_de_Vencimiento { get; set; }
        public double Tasa { get; set; }
        public string? CAEDEC_Prestamo { get; set; }
        public char Tiene_seguro_de_Desgravamen { get; set; }
        public Int64 Nro_de_Celular { get; set; }
    }
}
