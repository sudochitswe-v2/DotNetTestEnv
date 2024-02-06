
namespace CSDotNetTranning.ConsoleApp.Models
{
    public class BlogModel
    {
        public int BlogID { get; set; }
        public required string BlogTitle { get; set; }
        public required string BlogAuthor { get; set; }
        public required string BlogContent { get; set; }
    }
}
