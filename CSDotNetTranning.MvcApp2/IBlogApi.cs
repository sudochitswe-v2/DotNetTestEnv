using DotNetTrainingBatch3.MvcApp2.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTrainingBatch3.MvcApp2
{
    public interface IBlogApi
    {
        [Get("/api/Blog/blogs")]
        Task<List<BlogModel>> GetBlogs();

        [Get("/api/Blog/blogs?pageNo={pageNo}&pageSize={pageSize}")]
        Task<BlogResponseModel> GetBlogs(int pageNo, int pageSize);

        [Get("/api/Blog/blogs/{id}")]
        Task<BlogModel> GetBlog(int id);

        [Post("/api/Blog/blogs")]
        Task<string> CreateBlog(BlogModel blog);

        [Put("/api/Blog/blogs/{id}")]
        Task<string> UpdateBlog(int id, BlogModel blog);

        [Delete("/api/Blog/blogs/{id}")]
        Task<string> DeleteBlog(int id);
    }
}
