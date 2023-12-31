﻿using System.ComponentModel.DataAnnotations;

namespace CuBank.Models
{
    public class Operacion
    {
        [Key]
        [Required]
        public int OperacionId { get; set; }

        [Required(ErrorMessage = "El monto es requerido!")]
        [DataType(DataType.Currency)]
        public int Monto { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }

        public Usuario? User { get; set; }
    }
}
