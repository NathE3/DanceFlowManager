using RestAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Register
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Field required: isProfesor")]
        public bool IsProfesor { get; set; }

        [Required(ErrorMessage = "Field required: Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field required: Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Field required: UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field required: Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field required: Telefono")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "Field required: Password")]
        [PasswordValidation]
        public string Password { get; set; }
    }
}
