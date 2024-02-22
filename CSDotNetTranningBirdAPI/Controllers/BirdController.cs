using CSDotNetTranning.BirdApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSDotNetTranning.BirdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _url = "https://burma-project-ideas.vercel.app";
        private readonly string _resource = "birds";
        public BirdController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpClient.GetAsync($"{_url}/{_resource}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var birds = JsonConvert.DeserializeObject<List<BirdDataModel>>(json)!;
                var list = birds.Select(bird => bird.ConvertToViewModel(_url)).ToList();
                return Ok(list);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _httpClient.GetAsync($"{_url}/{_resource}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var bird = JsonConvert.DeserializeObject<BirdDataModel>(json)!;
                var birdVm = bird.ConvertToViewModel(_url);
                return Ok(birdVm);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
