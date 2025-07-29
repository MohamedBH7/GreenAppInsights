using System.ComponentModel.DataAnnotations;

namespace GreenAppInsights.Models
{
    public class EnergyEstimate
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MetricId { get; set; }
        public double EnergyMilliJoules { get; set; }

        public DateTime EstimatedAt { get; set; }
        public Metric Metric { get; set; }
    }
}
