using System.ComponentModel.DataAnnotations;

namespace Learn_Managment_System_Backend.DTO
{
    public class SignupDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120 años.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public required string Password { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]

        public required string PhoneNumber { get; set; }


        public required string UserName { get; set; }
    }
}