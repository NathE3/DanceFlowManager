using System.Text.Json.Serialization;


namespace InfoManager.Models
{

    public class UserDTO
    {
        [JsonPropertyName("status")]
        public int Status { get; set; } 

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("user")]
        public object User { get; set; }     
    }
}

