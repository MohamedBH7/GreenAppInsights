using GreenAppInsights.Data;
using GreenAppInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenAppInsights.Services
{
    public class OptimizationEngine
    {
        private readonly ApplicationDbContext _db;

        public OptimizationEngine(ApplicationDbContext db)
        {
            _db = db;
        }

        // Analyze metrics and generate optimization hints based on thresholds
        public async Task GenerateHintsAsync(Metric metric)
        {
            var hints = new List<OptimizationHint>();

            if (metric.DurationMs > 500) // Example threshold (500ms)
            {
                hints.Add(new OptimizationHint
                {
                    MetricId = metric.Id,
                    HintText = "High response time detected. Consider optimizing database queries or adding caching.",
                    Severity = "Warning",
                    CreatedAt = DateTime.Now
                });
            }

            if (metric.MemoryBytes > 10_000_000) // > ~10MB
            {
                hints.Add(new OptimizationHint
                {
                    MetricId = metric.Id,
                    HintText = "High memory usage detected. Review object lifetimes and memory allocations.",
                    Severity = "Warning",
                    CreatedAt = DateTime.Now
                });
            }

            if (!hints.Any()) // No issues found, add info hint
            {
                hints.Add(new OptimizationHint
                {
                    MetricId = metric.Id,
                    HintText = "Performance within expected parameters.",
                    Severity = "Info",
                    CreatedAt = DateTime.Now
                });
            }

            _db.OptimizationHints.AddRange(hints);
            await _db.SaveChangesAsync();
        }

        // Retrieve hints for a specific metric
        public async Task<List<OptimizationHint>> GetHintsByMetricIdAsync(Guid metricId)
        {
            return await _db.OptimizationHints
                .Where(h => h.MetricId == metricId)
                .ToListAsync();
        }
    }
}
