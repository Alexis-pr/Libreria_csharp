namespace AdminLibrary.DTOs;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Username { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirma tu contraseña")]
    public string ConfirmPassword { get; set; }
    
   
}
