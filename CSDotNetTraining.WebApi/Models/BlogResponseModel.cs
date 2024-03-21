namespace CSDotNetTraining.WebApi.Models
{
    public class ResponseModel<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool IsEndOfPage => PageNo >= PageCount;
        public IEnumerable<T> Data { get; set; }
    }
}
