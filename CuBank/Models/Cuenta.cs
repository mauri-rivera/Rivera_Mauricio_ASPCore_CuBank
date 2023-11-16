using System.ComponentModel.DataAnnotations;

namespace CuBank.Models
{
    public class Cuenta
    {
        [Key]
        [Required]
        public int CuentaId { get; set; }

        [Required]
        public int Monto { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }

        public Usuario? User { get; set; }
    }
}
