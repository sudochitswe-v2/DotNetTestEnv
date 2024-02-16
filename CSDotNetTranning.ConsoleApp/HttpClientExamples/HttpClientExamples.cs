using CSDotNetTranning.ConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.HttpClientExamples
{
    public class HttpClientExamples
    {
        private readonly string _resourceUrl = "https://localhost:7182/api/blog/blogs";
        public async Task Run()
        {
            //await Edit(11);
            //await Read();
            //await Create("New title", "Author", "Content");
            //await Update(11, "Update", "Update", "Update");
            await Delete(13);
        }
        private async Task Read()
        {
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var client = new HttpClient();
            Console.WriteLine($"data fetching... {DateTime.Now}");
            var response = await client.GetAsync(_resourceUrl);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"deserialization... {DateTime.Now}");
                string jsonStr = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<BlogModel>>(jsonStr);
                if (list is not null)
                {
                    foreach (var item in list)
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
        }
        private async Task Edit(int id)
        {
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var client = new HttpClient();
            Console.WriteLine($"data fetching... {DateTime.Now}");
            var response = await client.GetAsync($"{_resourceUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"deserialization... {DateTime.Now}");
                string jsonStr = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<BlogModel>(jsonStr);
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
            else
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
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
            string jsonBlog = JsonConvert.SerializeObject(blog);
            var httpContent = new StringContent(jsonBlog, Encoding.UTF8, MediaTypeNames.Application.Json);
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var client = new HttpClient();
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await client.PostAsync(_resourceUrl, httpContent);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        private async Task Update(int id, string title, string author, string content)
        {
            var blog = new BlogModel()
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };
            string jsonBlog = JsonConvert.SerializeObject(blog);
            var httpContent = new StringContent(jsonBlog, Encoding.UTF8, MediaTypeNames.Application.Json);
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var client = new HttpClient();
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await client.PutAsync($"{_resourceUrl}/{id}", httpContent);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        private async Task Delete(int id)
        {
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var client = new HttpClient();
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await client.DeleteAsync($"{_resourceUrl}/{id}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
