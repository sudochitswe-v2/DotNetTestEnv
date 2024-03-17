using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MissingHistoricalRecords.WebApi.Models
{
    [Table("tbl_Book")]
    public class BookModel
    {
        [Key]
        public int? BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public string BookAuthor { get; set; } = null!;
        public string BookCover { get; set; } = null!;
        public string BookCategory { get; set; } = null!;
        public string BookDescription { get; set; } = null!;

    }
}
