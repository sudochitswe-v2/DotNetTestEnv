using CSDotNetTranning.ConsoleApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.RestClientExamples
{
    public class RestClientExample
    {
        private readonly string _resourceUrl = "https://localhost:7182/api/blog/blogs";
        private readonly RestClient _client;
        public RestClientExample()
        {
            _client = new RestClient();
        }
        public async Task Run()
        {
            //await Edit(11);
            await Read();
            //await Create("New title", "Author", "Content");
            //await Update(11, "Update", "Update", "Update");
            //await Delete(13);
        }
        private async Task Read()
        {
            Console.WriteLine($"rest client starting... {DateTime.Now}");
            Console.WriteLine($"data fetching... {DateTime.Now}");
            var request = new RestRequest(_resourceUrl);
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"deserialization... {DateTime.Now}");
                string jsonStr = response.Content!;
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
            else
            {
                Console.WriteLine(response.Content);
            }
        }
        private async Task Edit(int id)
        {
            Console.WriteLine($"http client starting... {DateTime.Now}");
            var request = new RestRequest(_resourceUrl);
            Console.WriteLine($"data fetching... {DateTime.Now}");
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"deserialization... {DateTime.Now}");
                string jsonStr = response.Content!;
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
                Console.WriteLine(response.Content);
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
            var request = new RestRequest(_resourceUrl, Method.Post);
            request.AddJsonBody(blog);
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(response.Content!);
        }
        private async Task Update(int id, string title, string author, string content)
        {
            var blog = new BlogModel()
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };
            var request = new RestRequest(_resourceUrl, Method.Put);
            request.AddJsonBody(blog);
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(response.Content!);
        }
        private async Task Delete(int id)
        {
            var request = new RestRequest(_resourceUrl, Method.Delete);
            Console.WriteLine($"http client starting... {DateTime.Now}");
            Console.WriteLine($"data sending... {DateTime.Now}");
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(response.Content!);
        }
    }
}
