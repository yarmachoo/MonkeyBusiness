using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonkeyBuisness.Models.Filters.Movie;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieViewModel model)
    {
        string login = HttpContext.User.Identity.Name;

        var response = await _movieService.Create(model, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            //return RedirectToAction("Index", "Movie");
            return Ok(new { description = response.Description });
        }
        return View(model);
        //return BadRequest(new { description = response.Description });
    }
    [HttpPost]
    public async Task<IActionResult> MovieHandler(MovieFilter filter)
    {
        string login = HttpContext.User.Identity.Name;

        var response = await _movieService.GetMovies(filter, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Json(new { data = response.Data });
            //return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });

    }
    [Authorize]
    public IActionResult CreateMovie()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> EditMovie(long id)
    {

        var movie = await _movieService.GetMovieById(id);
        if (movie.Data == null)
        {
            return NotFound();
        }

        var model = new CreateMovieViewModel
        {
            Name = movie.Data.Name,
            Director = movie.Data.Director,
            Grade = movie.Data.Grade.ToString(),
            Theme = movie.Data.Theme,
            Description = movie.Data.Description,
            Note = movie.Data.Note,
            Id = movie.Data.Id.ToString(),
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditMovie(CreateMovieViewModel model)
    {
        var movie = await _movieService.ChangeMovie(model);
        if (movie.Data == null)
        {
            return NotFound();
        }

        if (true)
        {
            return RedirectToAction("Index", "Movie"); // Перенаправление, если редактирование прошло успешно
        }

    }
    [HttpGet]
    public async Task<IActionResult> DeleteMovie(long id)
    {

        var response = await _movieService.DeleteMovie(id);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return RedirectToAction("Index", "Movie");
            //return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }
}
