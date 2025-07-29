using GreenAppInsights.Services;
using GreenAppInsights.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenAppInsights.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ReportService _reportService;
        private readonly OptimizationEngine _optimizationEngine;

        public DashboardController(ReportService reportService, OptimizationEngine optimizationEngine)
        {
            _reportService = reportService;
            _optimizationEngine = optimizationEngine;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _reportService.GetDashboardReportAsync();

            var model = new DashboardViewModel
            {
                Reports = reports
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ReAnalyzeMetric(Guid metricId)
        {
            var metric = await _reportService.GetMetricByIdAsync(metricId);
            if (metric == null)
                return NotFound();

            // Clear existing hints for this metric (implement if needed)
            await _optimizationEngine.GenerateHintsAsync(metric);

            return RedirectToAction(nameof(Index));
        }
    }
}
