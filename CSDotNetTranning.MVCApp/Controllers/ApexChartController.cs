using CSDotNetTranning.MVCApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class ApexChartController : Controller
    {
        AppDbContext _context;
        public ApexChartController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult PieChart()
        {
            var series = new List<int> { 44, 55, 13, 43, 22 };
            var lables = new List<string> { "Team A", "Team B", "Team C", "Team D", "Team E" };
            var response = new ApexChartPieChartResponseModel()
            {
                Series = series,
                Lables = lables
            };
            return View(response);
        }
        public IActionResult DashedLineChart()
        {
            var lst = _context.PageStatistics.ToList();
            var model = new ApexChartDashedLineResponseModel();
            var lstSeries = new List<ApexChartDashedLineModel>();

            var lstSessionDuration = lst.Select(x => x.SessionDuration).ToList();
            var lstPageViews = lst.Select(x => x.PageViews).ToList();
            var lstTotalVisits = lst.Select(x => x.TotalVisits).ToList();
            var lstDate = lst.Select(x => x.CreatedDate.ToString("dd-MMM")).ToList();

            lstSeries.Add(new ApexChartDashedLineModel { name = "Session Duration", data = lstSessionDuration });
            lstSeries.Add(new ApexChartDashedLineModel { name = "Page Views", data = lstPageViews });
            lstSeries.Add(new ApexChartDashedLineModel { name = "Total Visits", data = lstTotalVisits });

            model.Series = lstSeries;
            model.Lables = lstDate;
            return View(model);
        }
        public IActionResult RadarChart()
        {
            
            var lst = _context.Radars.ToList();

            var model = new ApexChartRadarResponseModel();
            model.Series = lst.Select(x => x.Series).ToList();
            model.Lables = lst.Select(x => x.Month).ToList();

            return View(model);
        }
    }
}
