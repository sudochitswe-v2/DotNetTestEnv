using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MissingHistoricalRecords.WebApi.Models;
using MissingHistoricalRecords.WebApi.Repository;

namespace MissingHistoricalRecords.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private readonly DapperRepository _dapper;
        public DapperController(DapperRepository dapper)
        {
            _dapper = dapper;
        }
        [HttpGet("books")]
        public ActionResult<IEnumerable<BookModel>> GetBooks()
        {
            var books = _dapper.GetBooks();
            return Ok(books);
        }
        [HttpPost("books")]
        public ActionResult<IEnumerable<BookModel>> CrateBook(BookModel createModel)
        {
            var result = _dapper.CreateBook(createModel);
            var msg = result > 0 ? "Save success" : "Save fail";
            return Ok(msg);
        }
        [HttpGet("books/{id}")]
        public ActionResult<BookModel> GetBook(int id)
        {
            var book = _dapper.GetBook(id);
            if (book is null)
            {
                return NotFound("No record found.");
            }
            return Ok(book);
        }
        [HttpPut("books/{id}")]
        public IActionResult UpdateBook(int id, BookModel editModel)
        {
            var book = _dapper.GetBook(id);
            if (book is null)
            {
                return NotFound("No record found.");
            }
            var result = _dapper.UpdateBook(id, editModel);
            var msg = result > 0 ? "Update success" : "Update fail";
            return Ok(msg);
        }
        [HttpDelete("books/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _dapper.GetBook(id);
            if (book is null)
            {
                return NotFound("No record found");
            }
            var result = _dapper.DeleteBook(book);
            var msg = result > 0 ? "Delete success" : "Delete fail";
            return Ok(msg);
        }
    }
}
