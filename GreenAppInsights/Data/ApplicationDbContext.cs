using GreenAppInsights.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenAppInsights.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<EnergyEstimate> EnergyEstimates { get; set; }
        public DbSet<OptimizationHint> OptimizationHints { get; set; }
    }
}
