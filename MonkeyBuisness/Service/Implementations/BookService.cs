using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.DAL.Repositories;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Enum;
using MonkeyBuisness.Models.Extensions;
using MonkeyBuisness.Models.Filters.Book;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Book;
using MonkeyBuisness.Models.ViewModels.Note;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Service.Implementations;
public class BookService : IBookService
{
    private readonly IBaseRepository<BookEntity> _bookRepository;
    private readonly IBaseRepository<PersonEntity> _personRepository;
    private ILogger<BookService> _logger;
    public BookService(IBaseRepository<BookEntity> bookRepository, IBaseRepository<PersonEntity> personRepository, ILogger<BookService> logger)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _personRepository = personRepository;
    }
    public async Task<IBaseResponse<BookEntity>> Create(CreateBookViewModel model, string login)
    {
        try
        {
            model.Validate();
            _logger.LogInformation($"Запрос на создание книги: {model.Name}");

            var book = await _bookRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Author == model.Author);
            if (book != null)
            {
                return new BaseResponse<BookEntity>()
                {
                    Description = "Книга с таким названием и автором уже добавлена!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }
            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == login);
            if(person!=null)
            {
                id = person.Id;
            }
            model.Description = "fgergerge";
            model.Note = "kjjfhkrjfhwe";
            model.Grade = "3";
            book = new BookEntity()
            {
                PersonId = id,
                Name = model.Name,
                Author = model.Author,
                Description = model.Description,
                Note = model.Note,
                Theme = model.Theme,
                Grade = int.Parse(model.Grade)
            };
            await _bookRepository.Create(book);
            _logger.LogInformation($"Книга добавлена: {book.Name}");

            var personToAddBook = await _personRepository.GetAll()
            .Include(p => p.Notes) // Включаем список задач пользователя
            .FirstOrDefaultAsync(x => x.Login == login);

            // Добавляем созданную задачу в список задач пользователя
            personToAddBook.Books.Add(book);

            await _personRepository.Update(person);

            _logger.LogInformation($"Конспект добавлен в список пользовательских задач: {personToAddBook.Login}");


            return new BaseResponse<BookEntity>()
            {
                Description = $"Книга добавлена!",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[BookService.Create]: {ex.Message}");
            return new BaseResponse<BookEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }


    public async Task<IBaseResponse<IEnumerable<BookViewModel>>> GetBooks(BookFilter filter, string login)
    {
        try
        {
            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync (x => x.Login == login);
            if(person != null)
            {
                id = person.Id;
            }
            var books = await _bookRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Theme), x => x.Theme == filter.Theme)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Grade), x => x.Grade.ToString() == filter.Grade)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Author), x => x.Author == filter.Author)
                .Select(x => new BookViewModel()
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Theme = x.Theme,
                    Author = x.Author,
                    Grade = x.Grade.ToString(),
                    Description = x.Description
                })
                .ToListAsync();
            return new BaseResponse<IEnumerable<BookViewModel>>()
            {
                Data = books,
                StatusCode = StatusCode.OK
            };
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, $"[BookService.Create]: {ex.Message}");
            return new BaseResponse<IEnumerable<BookViewModel>>()
            {
                Description = $"{ex.Message}",
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<BookEntity>> GetBookById(long id)
    {
        try
        {
            var book = await _bookRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return new BaseResponse<BookEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            return new BaseResponse<BookEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = book
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[BookService.Create]: {ex.Message}");
            return new BaseResponse<BookEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<BookEntity>> ChangeBook(CreateBookViewModel model)
    {
        try
        {
            long id = long.Parse(model.Id);
            var book = await _bookRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return new BaseResponse<BookEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            BookEntity newBook = new BookEntity()
            {
                PersonId = book.PersonId,
                Name = model.Name,
                Author = model.Author,
                Description = model.Description,
                Note = model.Note,
                Theme = book.Theme,
                Person = book.Person,
                Grade = int.Parse(model.Grade)
            };
            await _bookRepository.Delete(book);
            await _bookRepository.Create(newBook);

            return new BaseResponse<BookEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = newBook
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[BookService.Create]: {ex.Message}");
            return new BaseResponse<BookEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<bool>> DeleteBook(long id)
    {
        try
        {

            var movie = await _bookRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена!"
                };
            }

            await _bookRepository.Delete(movie);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Задача завершена!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
}
