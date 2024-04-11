using CSDotNetTranning.MVCApp.Models;
using CSDotNetTranning.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class BlogAdoDotNetController : Controller
    {
        private readonly AdoDotNetService _adoDotNetService;

        public BlogAdoDotNetController(AdoDotNetService adoDotNetService)
        {
            _adoDotNetService = adoDotNetService;
        }

        public IActionResult Index()
        {
            var result = _adoDotNetService.Query<BlogModel>("SELECT * FROM tbl_blog");
            return View(result);
        }
    }
}
