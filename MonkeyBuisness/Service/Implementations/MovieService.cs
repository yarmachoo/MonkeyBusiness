using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.DAL.Repositories;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Enum;
using MonkeyBuisness.Models.Extensions;
using MonkeyBuisness.Models.Filters.Movie;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Service.Implementations;
public class MovieService : IMovieService
{
    private readonly IBaseRepository<MovieEntity> _movieRepository;
    private readonly IBaseRepository<PersonEntity> _personRepository;
    private ILogger<MovieService> _logger;
    public MovieService(IBaseRepository<MovieEntity> movieRepository, IBaseRepository<PersonEntity> personRepository, ILogger<MovieService> logger)
    {
        _logger = logger;
        _movieRepository = movieRepository;
        _personRepository = personRepository;
    }
    public async Task<IBaseResponse<MovieEntity>> Create(CreateMovieViewModel model, string login)
    {
        try
        {
            model.Validate();
            _logger.LogInformation($"Запрос на создание книги: {model.Name}");

            var movie = await _movieRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Director == model.Director);
            if (movie != null)
            {
                return new BaseResponse<MovieEntity>()
                {
                    Description = "Книга с таким названием и автором уже добавлена!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }
            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == login);
            if (person != null)
            {
                id = person.Id;
            }
            model.Description = "fgergerge";
            model.Note = "kjjfhkrjfhwe";
            model.Grade = "3";
            movie = new MovieEntity()
            {
                PersonId = id,
                Name = model.Name,
                Director = model.Director,
                Description = model.Description,
                Note = model.Note,
                Theme = model.Theme,
                Grade = int.Parse(model.Grade)
            };
            await _movieRepository.Create(movie);
            _logger.LogInformation($"Книга добавлена: {movie.Name}");

            var personToAddMovie = await _personRepository.GetAll()
            .Include(p => p.Notes) // Включаем список задач пользователя
            .FirstOrDefaultAsync(x => x.Login == login);

            // Добавляем созданную задачу в список задач пользователя
            personToAddMovie.Movies.Add(movie);

            await _personRepository.Update(person);

            _logger.LogInformation($"Конспект добавлен в список пользовательских задач: {personToAddMovie.Login}");


            return new BaseResponse<MovieEntity>()
            {
                Description = $"Книга добавлена!",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<MovieEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }


    public async Task<IBaseResponse<IEnumerable<MovieViewModel>>> GetMovies(MovieFilter filter, string login)
    {
        try
        {
            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == login);
            if (person != null)
            {
                id = person.Id;
            }
            var movies = await _movieRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Theme), x => x.Theme == filter.Theme)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Grade), x => x.Grade.ToString() == filter.Grade)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Director), x => x.Director == filter.Director)
                .Select(x => new MovieViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Theme = x.Theme,
                    Director = x.Director,
                    Grade = x.Grade.ToString(),
                    Description = x.Description
                })
                .ToListAsync();
            return new BaseResponse<IEnumerable<MovieViewModel>>()
            {
                Data = movies,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<IEnumerable<MovieViewModel>>()
            {
                Description = $"{ex.Message}",
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<MovieEntity>> GetMovieById(long id)
    {
        try
        {
            var movie = await _movieRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return new BaseResponse<MovieEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            return new BaseResponse<MovieEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = movie
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<MovieEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<MovieEntity>> ChangeMovie(CreateMovieViewModel model)
    {
        try
        {
            long id = long.Parse(model.Id);
            var movie = await _movieRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return new BaseResponse<MovieEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            MovieEntity newMovie = new MovieEntity()
            {
                PersonId = movie.PersonId,
                Name = model.Name,
                Director = model.Director,
                Description = model.Description,
                Note = model.Note,
                Theme = movie.Theme,
                Person = movie.Person,
                Grade = int.Parse(model.Grade)
            };
            await _movieRepository.Delete(movie);
            await _movieRepository.Create(newMovie);

            return new BaseResponse<MovieEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = newMovie
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<MovieEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<bool>> DeleteMovie(long id)
    {
        try
        {

            var movie = await _movieRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена!"
                };
            }

            await _movieRepository.Delete(movie);

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
