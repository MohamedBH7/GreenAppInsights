using GreenAppInsights.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GreenAppInsights.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly OptimizationEngine _optimizationEngine;

        public RecommendationsController(OptimizationEngine optimizationEngine)
        {
            _optimizationEngine = optimizationEngine;
        }

        // GET: /Recommendations?metricId={guid}
        public async Task<IActionResult> Index(Guid? metricId)
        {
            if (metricId == null || metricId == Guid.Empty)
            {
                return BadRequest("Metric ID is required to show recommendations.");
            }

            var hints = await _optimizationEngine.GetHintsByMetricIdAsync(metricId.Value);

            return View(hints);
        }
    }
}
