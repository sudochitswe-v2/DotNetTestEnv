using System;
using System.Threading.Channels;

namespace CSDotNetTranning.BirdApi.Models
{
    public class BirdDataModel
    {
        public int Id { get; set; }
        public string BirdMyanmarName { get; set; }
        public string BirdEnglishName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public BirdViewModel ConvertToViewModel(string mainUrl)
        {
            return new BirdViewModel
            {
                BirdId = this.Id,
                BirdName = this.BirdMyanmarName,
                Des = this.Description,
                PhotoUrl = $"{mainUrl}/{this.ImagePath}"
            };
        }
    }
    public class BirdViewModel
    {
        public int BirdId { get; set; }
        public string BirdName { get; set; }
        public string Des { get; set; }
        public string PhotoUrl { get; set; }
    }

}
