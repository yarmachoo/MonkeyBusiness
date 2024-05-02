using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Book;
using MonkeyBuisness.Models.Filters.Movie;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Book;
using MonkeyBuisness.Models.ViewModels.Movie;

namespace MonkeyBuisness.Service.Interfaces;

public interface IMovieService
{
    Task<IBaseResponse<MovieEntity>> Create(CreateMovieViewModel model, string login);
    Task<IBaseResponse<IEnumerable<MovieViewModel>>> GetMovies(MovieFilter filter, string login);
    Task<IBaseResponse<MovieEntity>> ChangeMovie(CreateMovieViewModel model);
    Task<IBaseResponse<bool>> DeleteMovie(long id);
    Task<IBaseResponse<MovieEntity>> GetMovieById(long id);
}
