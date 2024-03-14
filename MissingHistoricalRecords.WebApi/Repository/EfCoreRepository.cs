using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MissingHistoricalRecords.WebApi.Models;

namespace MissingHistoricalRecords.WebApi.Repository
{
    public class EfCoreRepository
    {
        private readonly AppDbContext _appDbContext;
        public EfCoreRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<BookModel> GetBooks()
        {
            return _appDbContext.Books.AsNoTracking().ToArray().OrderByDescending(book => book.BookId);
        }
        public BookModel? GetBook(int id)
        {
            return _appDbContext.Books.FirstOrDefault(book => book.BookId == id);
        }
        public int CreateBook(BookModel book)
        {
            _appDbContext.Books.Add(book);
           return _appDbContext.SaveChanges();
        }
        public int UpdateBook(int bookId,BookModel editBook)
        {
           var existBook = _appDbContext.Books.FirstOrDefault(book => book.BookId == bookId);
           if(editBook is null)
            {
                return 0;
            }
            editBook.BookTitle = editBook.BookTitle;
            existBook.BookAuthor = editBook.BookAuthor;
            existBook.BookCover = editBook.BookCover;
            existBook.BookCategory = editBook.BookCategory;
            existBook.BookDescription = editBook.BookDescription;
            return _appDbContext.SaveChanges();
            
        }
        public int DeleteBook(BookModel deleteModel)
        {
            _appDbContext.Remove(deleteModel);
           return _appDbContext.SaveChanges();
        }
    }
}
