using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSDotNetTranning.MVCApp.Models
{
    [Table("Tbl_RadarChart")]
    public class RadarModel
    {
        [Key]
        public int Id { get; set; }
        public string Month { get; set; }
        public int Series { get; set; }
    }
}
