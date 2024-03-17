using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MissingHistoricalRecords.WebApi.Models
{
    [Table("tbl_Content")]
    public class ContentModel
    {
        [Key]
        public int? ContentId { get; set; }
        public int BookId { get; set; }
        public int PageNo { get; set; }
        public string ContentText { get; set; } = null!;
    }
}
