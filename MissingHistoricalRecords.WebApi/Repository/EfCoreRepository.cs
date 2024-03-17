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
        public int UpdateBook(int bookId, BookModel editBook)
        {
            var existBook = _appDbContext.Books.FirstOrDefault(book => book.BookId == bookId);
            if (editBook is null)
            {
                return 0;
            }
            existBook.BookTitle = editBook.BookTitle;
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
        public IEnumerable<ContentModel> GetBookContents(int bookId, int? pageNo)
        {

            var contents = _appDbContext.BookContents
                .Where(content => content.BookId == bookId && (pageNo == null || content.PageNo == pageNo))
                .ToArray()
                .OrderBy(content => content.PageNo);
            return contents;
        }
        public ContentModel? GetCotent(int contentId)
        {
            var content = _appDbContext.BookContents
                .FirstOrDefault(content => content.ContentId == contentId);
            return content;
        }
        public int CreateContent(ContentModel createModel)
        {
            _appDbContext.Add(createModel);
            return _appDbContext.SaveChanges();
        }
        public int UpdateContent(int contentId, ContentModel editModel)
        {
            var existContent = _appDbContext.BookContents.Find(contentId);
            if (existContent is null) return 0;
            existContent.ContentText = editModel.ContentText;
            return _appDbContext.SaveChanges();
        }
        public int DeleteContent(ContentModel deleteModel)
        {
            _appDbContext.Remove(deleteModel);
            return _appDbContext.SaveChanges();
        }
    }
}
