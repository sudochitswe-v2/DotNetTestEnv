using CSDotNetTranning.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.EFCoreExamples
{
    public class EFCoreExample
    {
        private readonly AppDbContext _dbContext = new();
        public void Read()
        {
            var blogs = _dbContext.Blogs.ToList();
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.BlogID);
                Console.WriteLine(blog.BlogTitle);
                Console.WriteLine(blog.BlogContent);
                Console.WriteLine(blog.BlogAuthor);
            }
        }
        public void Edit(int id)
        {
            BlogModel? item = _dbContext.Blogs.FirstOrDefault(item => item.BlogID == id);
            if (item is null)
            {
                Console.WriteLine("No data found.");
                return;
            }
            Console.WriteLine(item.BlogID);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
        }
        public void Create(string title, string author, string content)
        {
            var blog = new BlogModel()
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };

            _dbContext.Blogs.Add(blog);
            int result = _dbContext.SaveChanges();

            string message = result > 0 ? "Save Success" : "Save Fail";

            Console.WriteLine(message);
        }
        public void Update(int id, string title, string author, string content)
        {
            var item = _dbContext.Blogs.FirstOrDefault(item => item.BlogID == id);
            if (item is null)
            {
                Console.WriteLine("No data found.");
                return;
            }

            item.BlogTitle = title;
            item.BlogAuthor = author;
            item.BlogContent = content;
            int result = _dbContext.SaveChanges();

            string message = result > 0 ? "Update Success" : "Update Fail";

            Console.WriteLine(message);
        }
        public void Delete(int id)
        {
            var item = _dbContext.Blogs.FirstOrDefault(item => item.BlogID == id);
            if (item is null)
            {
                Console.WriteLine("No data found.");
                return;
            }

            _dbContext.Blogs.Remove(item);
            int result = _dbContext.SaveChanges();

            string message = result > 0 ? "Delete Success" : "Delete Fail";

            Console.WriteLine(message);
        }
        public void BulkCreate(int length)
        {
            for (int i = 1; i < length; i++)
            {
                Create($"Title{i}",$"Author{i}", $"Content{i}");    
            }
        }
    }
}
