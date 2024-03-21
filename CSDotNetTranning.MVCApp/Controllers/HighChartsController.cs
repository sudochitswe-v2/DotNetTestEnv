using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class HighChartsController : Controller
    {
        public IActionResult DonutChart()
        {
            return View();
        }
    }
}
