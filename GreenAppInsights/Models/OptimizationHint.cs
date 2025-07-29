using System;
using System.ComponentModel.DataAnnotations;

namespace GreenAppInsights.Models
{
    public class OptimizationHint
    {
        [Key]
        public Guid Guid { get; set; }

        public Guid MetricId { get; set; }

        [Required]
        [MaxLength(300)]
        public string HintText { get; set; }

        public string Severity { get; set; } // Info, Warning, Critical

        public DateTime CreatedAt { get; set; }

        public Metric Metric { get; set; }
    }
}
