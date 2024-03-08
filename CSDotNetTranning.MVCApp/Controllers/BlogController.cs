using CSDotNetTranning.MVCApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTranning.MVCApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;
        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [ActionName("Index")]
        public IActionResult BlogIndex()
        {
          var blogs =  _dbContext.Blogs.ToList();
            return View("BlogIndex",blogs);
        }
        [ActionName("Edit")]
        public IActionResult BlogEdit(int id)
        {
            var blog = _dbContext.Blogs.Find(id);
            if(blog is null)
            {
                return Redirect("/Blog");
            }
            return View("BlogEdit",blog);
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
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return Redirect("/Blog");
        }
        [HttpPost]
        [ActionName("Update")]
        public IActionResult BlogUpdate(int id,BlogModel blog)
        {
           var existBlog = _dbContext.Blogs.Find(id);
            if(existBlog is null) {
                return Redirect("/Blog");
            }
            existBlog.BlogTitle = blog.BlogTitle;
            existBlog.BlogAuthor = blog.BlogAuthor;
            existBlog.BlogContent = blog.BlogContent;
            _dbContext.SaveChanges();
            return Redirect("/Blog");
        }
        [ActionName("Delete")]
        public IActionResult BlogDelete(int id)
        {
            var blog = _dbContext.Blogs.Find(id);
            if (blog is null)
            {
                return Redirect("/Blog");
            }
            _dbContext.Blogs.Remove(blog);
            _dbContext.SaveChanges();
            return Redirect("/Blog");
        }
    }
}
