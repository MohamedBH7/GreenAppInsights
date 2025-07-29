using GreenAppInsights.Data;
using GreenAppInsights.Models;
using System;
using System.Threading.Tasks;

namespace GreenAppInsights.Services
{
    public class EnergyEstimator
    {
        private readonly ApplicationDbContext _db;

        public EnergyEstimator(ApplicationDbContext db)
        {
            _db = db;
        }

        // Simple example: estimate energy based on duration and memory usage
        public async Task EstimateAndSaveAsync(Metric metric)
        {
            var energyEstimate = new EnergyEstimate
            {
                Id = Guid.NewGuid(),
                MetricId = metric.Id,
                EnergyMilliJoules = CalculateEnergy(metric),
                EstimatedAt = DateTime.Now
            };

            await _db.EnergyEstimates.AddAsync(energyEstimate);
            await _db.SaveChangesAsync();
        }

        private double CalculateEnergy(Metric metric)
        {
            // Placeholder calculation: adjust with your formula
            return (metric.DurationMs * 0.5) + (metric.MemoryBytes * 0.0001);
        }
    }
}
