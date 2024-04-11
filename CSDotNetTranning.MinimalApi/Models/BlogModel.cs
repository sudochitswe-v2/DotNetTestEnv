using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDotNetTranning.MinimalApi.Models
{
    [Table("Tbl_Blog")]
    public class BlogModel
    {
        [Key]
        public int BlogID { get; set; }
        public string BlogTitle { get; set; } = null!;
        public string BlogAuthor { get; set; } = null!;
        public string BlogContent { get; set; } = null!;
    }
}
