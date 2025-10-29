using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Login
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Field required: Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field required: Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field required: isProfesor")]
        public bool IsProfesor { get; set; }
    }
}
