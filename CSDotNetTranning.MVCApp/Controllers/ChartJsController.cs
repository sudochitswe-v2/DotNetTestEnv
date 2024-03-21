using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class ChartJsController : Controller
    {
        public IActionResult BarChart()
        {
            return View();
        }
    }
}
