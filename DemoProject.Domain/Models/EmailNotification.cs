
using System.Text.Json.Serialization;

namespace DemoProject.Domain.Models
{
    public class EmailNotification
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
