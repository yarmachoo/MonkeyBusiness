using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonkeyBuisness.Models.Filters.Note;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Models.ViewModels.Note;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Controllers;

public class NoteController : Controller
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteViewModel model)
    {
        string login = HttpContext.User.Identity.Name;

        var response = await _noteService.Create(model, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }

    [HttpPost]
    public async Task<IActionResult> NoteHandler(NoteFilter filter)
    {
        string login = HttpContext.User.Identity.Name;
        var response = await _noteService.GetNotes(filter, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Json(new { data = response.Data });
        }
        return BadRequest(new { description = "Failed to fetch tasks." });
        //return Json(new { data  = response.Data});
    }
    [Authorize]
    public IActionResult CreateNote()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> EditNote(long id)
    {

        var note = await _noteService.GetNoteById(id);
        if (note.Data == null)
        {
            return NotFound();
        }

        var model = new CreateNoteViewModel
        {
            Name = note.Data.Name,
            Subject = note.Data.Subject,
            Theme = note.Data.Theme,
            Note = note.Data.Note,
            Id = note.Data.Id.ToString(),
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditNote(CreateNoteViewModel model)
    {
        var note = await _noteService.ChangeNote(model);
        if (note.Data == null)
        {
            return NotFound();
        }

        if (true)
        {
            return RedirectToAction("Index", "Note"); // Перенаправление, если редактирование прошло успешно
        }

    }
    public async Task<IActionResult> DeleteNote(long id)
    {

        var response = await _noteService.DeleteNote(id);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return RedirectToAction("Index", "Note");
            //return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }

}
