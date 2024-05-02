using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Book;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Book;

namespace MonkeyBuisness.Service.Interfaces;
public interface IBookService
{
    Task<IBaseResponse<BookEntity>> Create(CreateBookViewModel model,string login);
    Task<IBaseResponse<IEnumerable<BookViewModel>>> GetBooks(BookFilter filter, string login);
    Task<IBaseResponse<BookEntity>> ChangeBook(CreateBookViewModel model);
    Task<IBaseResponse<BookEntity>> GetBookById(long id);
    Task<IBaseResponse<bool>> DeleteBook(long id);
}
