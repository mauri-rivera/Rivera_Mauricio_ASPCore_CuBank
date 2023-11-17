using System.ComponentModel.DataAnnotations;

namespace CuBank.Models
{
    public class Historial
    {
        [Key]
        public int HistorialId { get; set; }

        public int HUsuarioId { get; set; }

        public int HOperacionId { get; set; }

        public int HCantidad { get; set; }

        public DateTime HUpdatedAt { get; set; } = DateTime.Now;
    }
}
