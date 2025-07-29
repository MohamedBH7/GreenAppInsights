using System.ComponentModel.DataAnnotations;

namespace GreenAppInsights.Models
{
    public class Metric
    {
        [Key]
        public Guid Id { get; set; }

        public string Endpoint { get; set; }
        public double DurationMs { get; set; }
        public long MemoryBytes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
