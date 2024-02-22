using CSDotNetTranning.ConsoleApp.Models;
using Refit;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.RefitExamples
{
    public class RefitExample
    {
        private readonly string _url = "https://localhost:7182";
        private readonly IBlogApi _blogApis;
        public RefitExample()
        {
            _blogApis = RestService.For<IBlogApi>(_url);
        }
        public async Task Run()
        {
            //await Read();
            //await Create("title", "aut", "cont");
            await Edit(14);
            await Edit(500);
        }
        private async Task Read()
        {
            Console.WriteLine($"data fetching... {DateTime.Now}");
            var response = await _blogApis.GetBlogs();
            if (response is not null)
            {
                foreach (var item in response)
                {
                    Console.WriteLine(item.BlogID);
                    Console.WriteLine(item.BlogTitle);
                    Console.WriteLine(item.BlogAuthor);
                    Console.WriteLine(item.BlogContent);
                    Console.WriteLine("----------------");
                }
                Console.WriteLine($"completed... {DateTime.Now}");
            }
        }


        private async Task Edit(int id)
        {
            try
            {
                Console.WriteLine($"data fetching... {DateTime.Now}");
                var item = await _blogApis.GetBlog(id);

                if (item is null)
                {
                    Console.WriteLine("No data found");
                    return;
                }
                Console.WriteLine(item.BlogID);
                Console.WriteLine(item.BlogTitle);
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogContent);
                Console.WriteLine("----------------");
                Console.WriteLine($"completed... {DateTime.Now}");
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private async Task Create(string title, string author, string content)
        {
            var blog = new BlogModel()
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };
            var response = await _blogApis.CreateBlog(blog);
            Console.WriteLine(response);
        }
        private async Task Update(int id, string title, string author, string content)
        {
            try
            {
                var blog = new BlogModel()
                {
                    BlogTitle = title,
                    BlogAuthor = author,
                    BlogContent = content
                };

                Console.WriteLine($"data sending... {DateTime.Now}");
                var response = await _blogApis.UpdateBlog(id, blog);
                Console.WriteLine(response);
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        private async Task Delete(int id)
        {
            try
            {
                Console.WriteLine($"data sending... {DateTime.Now}");
                var response = await _blogApis.DeleteBlog(id);
                Console.WriteLine(response);
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }

}
