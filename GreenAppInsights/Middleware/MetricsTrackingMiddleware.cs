using GreenAppInsights.Data;
using GreenAppInsights.Models;
using GreenAppInsights.Services;
using System.Diagnostics;

namespace GreenAppInsights.Middleware
{
    public class MetricsTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        public MetricsTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext db)
        {
            var stopwatch = Stopwatch.StartNew();
            var startMemory = GC.GetTotalMemory(true);

            await _next(context);

            stopwatch.Stop();
            var endMemory = GC.GetTotalMemory(false);

            var metric = new Metric
            {
                Endpoint = context.Request.Path,
                DurationMs = stopwatch.Elapsed.TotalMilliseconds,
                MemoryBytes = endMemory - startMemory,
                Timestamp = DateTime.Now
            };

            db.Metrics.Add(metric);
            await db.SaveChangesAsync();

            var optimizationEngine = new OptimizationEngine(db);
            await optimizationEngine.GenerateHintsAsync(metric);

            // Generate energy estimate
            var energyEstimator = new EnergyEstimator(db);
            await energyEstimator.EstimateAndSaveAsync(metric);
        }
    }
}
