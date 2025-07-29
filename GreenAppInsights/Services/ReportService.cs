using GreenAppInsights.Data;
using GreenAppInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenAppInsights.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _db;

        public ReportService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get metrics with energy estimates and hints for dashboard
        public async Task<List<DashboardMetricReport>> GetDashboardReportAsync(int top = 50)
        {
            var query = await (from m in _db.Metrics
                               join e in _db.EnergyEstimates on m.Id equals e.MetricId into energyGroup
                               from e in energyGroup.DefaultIfEmpty()
                               select new DashboardMetricReport
                               {
                                   MetricId = m.Id,
                                   Endpoint = m.Endpoint,
                                   DurationMs = m.DurationMs,
                                   MemoryBytes = m.MemoryBytes,
                                   EnergyMilliJoules = e != null ? e.EnergyMilliJoules : 0,
                                   Timestamp = m.Timestamp
                               })
                              .OrderByDescending(x => x.Timestamp)
                              .Take(top)
                              .ToListAsync();

            return query;
        }

        public async Task<Metric> GetMetricByIdAsync(Guid metricId)
        {
            return await _db.Metrics.FirstOrDefaultAsync(m => m.Id == metricId);
        }
    }

    // Helper DTO for dashboard reporting
    public class DashboardMetricReport
    {
        public Guid MetricId { get; set; }
        public string Endpoint { get; set; }
        public double DurationMs { get; set; }
        public long MemoryBytes { get; set; }
        public double EnergyMilliJoules { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
