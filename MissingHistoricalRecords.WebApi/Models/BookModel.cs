using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MissingHistoricalRecords.WebApi.Models
{
    [Table("tbl_Book")]
    public class BookModel
    {
        [Key]
        public int? BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookCover { get; set; }
        public string BookCategory { get; set; }
        public string BookDescription { get; set; }

    }
}
