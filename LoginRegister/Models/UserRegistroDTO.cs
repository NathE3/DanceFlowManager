using System.Text.Json.Serialization;

namespace InfoManager.Models
{
    public class UserRegistroDTO
    {
        [JsonPropertyName("isProfesor")]
        public bool IsProfesor { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apellido")]
        public string apellido { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
