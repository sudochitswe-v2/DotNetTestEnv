using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSDotNetTranning.MVCApp.Models
{
    [Table("Tbl_PageStatistics")]
    public class PageStatisticsModel
    {
        [Key]
        public int PageStatisticsId { get; set; }
        public int SessionDuration { get; set; }
        public int PageViews { get; set; }
        public int TotalVisits { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
