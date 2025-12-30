

using System.Text.Json.Serialization;

namespace InfoManager.Models
{
    public class LoginDTO 
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("isProfesor")]
        public bool IsProfesor { get; set; }

        [JsonIgnore]
        public string Token { get; set; }
    }
}
