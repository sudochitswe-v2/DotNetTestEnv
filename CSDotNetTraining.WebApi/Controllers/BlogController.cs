using CSDotNetTranning.WebApi.EFCoreExamples;
using CSDotNetTranning.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSDotNetTraining.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BlogController()
        {
            _db = new AppDbContext();
        }
        [HttpGet("blogs")]
        public IActionResult GetBlogs()
        {
            var result = _db.Blogs.OrderByDescending(blog => blog.BlogID);
            return Ok(result);
        }
        [HttpPost("blogs")]
        public IActionResult CreateBlog(BlogModel blog)
        {
            _db.Blogs.Add(blog);
            var result = _db.SaveChanges();
            var message = result > 0 ? "Success" : "Fail";
            return Ok(message);
        }
        [HttpPut("blogs/{id}")]
        public IActionResult UpdateBlog(int id, BlogModel blog)
        {
            var editModel = _db.Blogs.FirstOrDefault(blog => blog.BlogID == id);
            if(editModel is null)
            {
                return NotFound("No data found");
            }
            editModel.BlogTitle = blog.BlogTitle;
            editModel.BlogAuthor = blog.BlogAuthor;
            editModel.BlogContent = blog.BlogContent;
            var result = _db.SaveChanges();
            var message = result > 0 ? "Success" : "Fail";
            return Ok(message);
        }
        [HttpGet("blogs/{id}")]
        public IActionResult GetBlog(int id)
        {
            var result = _db.Blogs.FirstOrDefault(blog => blog.BlogID == id);
            if(result is null)
            {
                return NotFound("No data found");
            }
            return Ok(result);
        }
        [HttpDelete("blogs/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            var deleteModel = _db.Blogs.FirstOrDefault(blog => blog.BlogID == id);
            if(deleteModel is null)
            {
                return NotFound("No data found");
            }
            _db.Remove(deleteModel);
            var result = _db.SaveChanges();
            var message = result > 0 ? "Success" : "Fail";
            return Ok(message);
        }
    }
}
