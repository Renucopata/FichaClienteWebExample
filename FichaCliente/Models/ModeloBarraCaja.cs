namespace FichaCliente.Models
{
    public class ModeloBarraCaja
    {
        public Int64 name { get; set; }
        public decimal y { get; set; }

        public ModeloBarraCaja(Int64 cabecera, decimal valor)
        {
            this.name = cabecera;
            this.y = valor;
        }
    }
}
