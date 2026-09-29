using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.ViewModels.Auth;

public sealed class AdminLoginViewModel
{
    [Required(ErrorMessage = "Ingresa el correo administrativo.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [Display(Name = "Correo")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa la contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
}
