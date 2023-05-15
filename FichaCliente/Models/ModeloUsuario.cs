using System.ComponentModel.DataAnnotations;

namespace FichaCliente.Models
{
    public class ModeloUsuario
    {
        [Required(ErrorMessage ="Usuario o contraseña incorrectos")]
        public String? login { get; set; }

        public String? cargo { get; set; }
        public Int64 codigo_agenda { get; set; }
    }
}
