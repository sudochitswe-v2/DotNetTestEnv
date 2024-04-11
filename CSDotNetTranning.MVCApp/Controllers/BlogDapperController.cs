using CSDotNetTranning.MVCApp.Models;
using CSDotNetTranning.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class BlogDapperController : Controller
    {
        private readonly DapperService _dapperService;

        public BlogDapperController(DapperService dapperService)
        {
            _dapperService = dapperService;
        }

        public IActionResult Index()
        {
            var sql = "SELECT * FROM tbl_blog";
            var lst = _dapperService.Query<BlogModel>(sql);
            return View(lst);
        }
    }
}
