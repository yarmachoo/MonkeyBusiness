using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MonkeyBuisness.Service.Interfaces;
using MonkeyBuisness.Models.ViewModels.Book;
using MonkeyBuisness.Models.Filters.Book;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Response;

namespace MonkeyBuisness.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            string login = HttpContext.User.Identity.Name;

            var response = await _bookService.Create(model, login);
            if(response.StatusCode == Models.Enum.StatusCode.OK)
            {
                //return RedirectToAction("Index", "Book");
                return Ok(new {description =  response.Description});
            }
            return View(model);
            //return BadRequest(new { description = response.Description });
        }
        [HttpPost]
        public async Task<IActionResult> BookHandler(BookFilter filter)
        {
            string login = HttpContext.User.Identity.Name;

            var response = await _bookService.GetBooks(filter, login);
            if (response.StatusCode == Models.Enum.StatusCode.OK)
            {
                return Json(new { data = response.Data });
                //return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        
        }
        [Authorize]
        public IActionResult CreateBook()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditBook(long id)
        {

            var book = await _bookService.GetBookById(id);
            if (book.Data == null)
            {
                return NotFound();
            }

            var model = new CreateBookViewModel
            {
                Name = book.Data.Name,
                Author = book.Data.Author,
                Grade = book.Data.Grade.ToString(),
                Theme = book.Data.Theme,
                Description = book.Data.Description,
                Note = book.Data.Note,
                Id = book.Data.Id.ToString(),
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(CreateBookViewModel model)
        {
            var book = await _bookService.ChangeBook(model);
            if (book.Data == null)
            {
                return NotFound();
            }

            if (true)
            {
                return RedirectToAction("Index", "Book"); // Перенаправление, если редактирование прошло успешно
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBook(long id)
        {

            var response = await _bookService.DeleteBook(id);
            if (response.StatusCode == Models.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index", "Book");
                //return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }
    }
}
