using CSDotNetTranning.ConsoleApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.RefitExamples
{
    public interface IBlogApi
    {
        [Get("/api/Blog/blogs")]
        Task<List<BlogModel>> GetBlogs();
        [Get("/api/Blog/blogs/{id}")]
        Task<BlogModel> GetBlog(int id);
        [Post("/api/Blog/blogs")]
        Task<string> CreateBlog(BlogModel model);
        [Put("/api/Blog/blogs/{id}")]
        Task<string> UpdateBlog(int id, BlogModel model);
        [Delete("/api/Blog/blogs/{id}")]
        Task<string> DeleteBlog(int id);

    }
}
