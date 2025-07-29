using GreenAppInsights.Data;
using GreenAppInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenAppInsights.Services
{
    public class MetricsCollector
    {
        private readonly ApplicationDbContext _db;

        public MetricsCollector(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Metric> RecordAsync(string endpoint, double durationMs, long memoryBytes)
        {
            var metric = new Metric
            {
                Endpoint = endpoint,
                DurationMs = durationMs,
                MemoryBytes = memoryBytes,
                Timestamp = DateTime.Now
            };

            _db.Metrics.Add(metric);
            await _db.SaveChangesAsync();
            return metric;
        }

    }
}
// This class is responsible for collecting and recording metrics such as endpoint performance and memory usage.