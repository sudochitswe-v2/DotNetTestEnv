using CSDotNetTranning.MVCApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class BlogAjaxController : Controller
    {
        private readonly AppDbContext _context;
        public BlogAjaxController(AppDbContext context)
        {
            _context = context;
        }
        // https://localhost:7021/Blog/Index
        [ActionName("Index")]
        public IActionResult BlogIndex()
        {
            List<BlogModel> lst = _context.Blogs.ToList();
            return View("BlogIndex", lst);
        }

        // https://localhost:7021/Blog/Edit/1
        // https://localhost:7021/Blog/Edit?id=1
        [ActionName("Edit")]
        public IActionResult BlogEdit(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogID == id);
            if (item is null)
            {
                return Redirect("/Blog");
            }

            return View("BlogEdit", item);
        }

        [ActionName("Create")]
        public IActionResult BlogCreate()
        {
            return View("BlogCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult BlogSave(BlogModel blog)
        {
            _context.Blogs.Add(blog);
            int result = _context.SaveChanges();
            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            var response = new BlogMessageResponseModel(
                IsSuccess: result > 0,
                Message: message
                );

            return Json(response);
        }

        [HttpPost]
        [ActionName("Update")]
        public IActionResult BlogUpdate(int id, BlogModel blog)
        {
            var response = new BlogMessageResponseModel(false, "");
            var item = _context.Blogs.FirstOrDefault(blog => blog.BlogID == id);
            if (item is null)
            {
                return Json(response with { IsSuccess = false, Message = "No record found" });
            }

            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;

            int result = _context.SaveChanges();
            string message = result > 0 ? "Update Success." : "Update Fai.";
            return Json(response with { IsSuccess = result > 0, Message = message });
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult BlogDelete(BlogModel blog)
        {
            var response = new BlogMessageResponseModel(false, "");
            var item = _context.Blogs.FirstOrDefault(blog => blog.BlogID == blog.BlogID);
            if (item is null)
            {
                return Json(response with { IsSuccess = false, Message = "No record found" });
            }

            _context.Blogs.Remove(item);
            int result = _context.SaveChanges();
            string message = result > 0 ? "Delete Success." : "Delete Fail.";
            return Json(response with { IsSuccess = result > 0, Message = message });
        }
    }
}
