using RestAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Profesores
{
    public class ProfesorRegistrationDTO
    {
        [Required(ErrorMessage = "Field required: Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field required: UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field required: Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field required: Password")]

        [PasswordValidation]
        public string Password { get; set; }

        public string Estado { get; set; }
    }
}
