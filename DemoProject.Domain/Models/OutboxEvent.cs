using System;
using System.Collections.Generic;
namespace DemoProject.Domain.Models
{
    public class OutboxEvent
    {
        public Guid Id { get; set; } = new Guid();

        public string Type { get; set; }

        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}
