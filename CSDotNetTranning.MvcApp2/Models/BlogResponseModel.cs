﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNetTrainingBatch3.MvcApp2.Models
{
    public class BlogResponseModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool IsEndOfPage => PageNo >= PageCount;
        public List<BlogModel> Data { get; set; } = null!;
    }

    [Table("Tbl_Blog")]
    public class BlogModel
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogTitle { get; set; } = null!;
        public string BlogAuthor { get; set; } = null!;
        public string BlogContent { get; set; }
    }
}
