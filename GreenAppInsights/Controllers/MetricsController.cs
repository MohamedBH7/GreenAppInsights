using GreenAppInsights.Data;
using GreenAppInsights.Models;
using GreenAppInsights.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GreenAppInsights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MetricsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Metrics
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var metrics = await _db.Metrics.ToListAsync();
            return Ok(metrics);
        }

        // GET: api/Metrics/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var metric = await _db.Metrics.FindAsync(id);
            if (metric == null)
                return NotFound();

            return Ok(metric);
        }

        // POST: api/Metrics
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Metric metric)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            metric.Id = Guid.NewGuid();
            metric.Timestamp = DateTime.Now;

            await _db.Metrics.AddAsync(metric);
            await _db.SaveChangesAsync();

            // Generate optimization hints
            var optimizationEngine = new OptimizationEngine(_db);
            await optimizationEngine.GenerateHintsAsync(metric);

            // Generate energy estimate
            var energyEstimator = new EnergyEstimator(_db);
            await energyEstimator.EstimateAndSaveAsync(metric);

            return CreatedAtAction(nameof(GetById), new { id = metric.Id }, metric);
        }

        // Delete : api/Metrics/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var metric = await _db.Metrics.FindAsync(id);
            if (metric == null)
                return NotFound();

            _db.Metrics.Remove(metric);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("DeleteALL")]
        public async Task<IActionResult> DeleteAll()
        {

            await _db.OptimizationHints.ExecuteDeleteAsync();
            await _db.EnergyEstimates.ExecuteDeleteAsync();
            await _db.Metrics.ExecuteDeleteAsync();
            await _db.SaveChangesAsync();
            return Ok();

        }
    }
}
