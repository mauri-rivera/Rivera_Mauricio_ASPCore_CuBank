#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace CuBank.DAO
{
    public class Login
    {
        [Required(ErrorMessage = "El correo electrónico es requerido!")]
        [EmailAddress(ErrorMessage = "Debe proporcionar un email válido!")]
        public string EmailLogin { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida!")]
        [DataType(DataType.Password)]
        public string PasswordLogin { get; set; }
    }
}
