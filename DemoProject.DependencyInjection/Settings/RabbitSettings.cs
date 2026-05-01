using System.Text.Json.Serialization;

namespace DemoProject.DependencyInjection.Settings
{
    public class RabbitSettings
    {
        [JsonPropertyName("url")]
        public string HostUrl { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
